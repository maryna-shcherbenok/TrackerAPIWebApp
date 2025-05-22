const uri = '/api/healthrecords';
const moodUri = '/api/moodoptions';
const tagUri = '/api/tags';
const medUri = '/api/medications';

let records = [];
let moods = [];
let tags = [];
let medications = [];
let takenDates = [];

let tagChoices;
let medicationChoices;
let moodChoices;

function getRecords() {
    fetch(uri)
        .then(res => res.json())
        .then(data => {
            records = data;
            displayRecords();
        });
}

function getMoods() {
    fetch(moodUri)
        .then(res => res.json())
        .then(data => {
            moods = data;
            populateMoodSelect();
        });
}

function populateMoodSelect() {
    const select = document.getElementById('moodOptionId');
    if (!select) return;

    select.innerHTML = '<option value="">Оберіть настрій</option>';
    moods.forEach(mood => {
        const option = document.createElement('option');
        option.value = mood.id;
        option.textContent = mood.name;
        select.appendChild(option);
    });

    if (moodChoices) moodChoices.destroy();
    moodChoices = new Choices(select, {
        placeholderValue: 'Оберіть настрій',
        searchEnabled: false,
        itemSelectText: '',
        shouldSort: false
    });
}

function getTags() {
    fetch(tagUri)
        .then(res => res.json())
        .then(data => {
            tags = data;
            const select = document.getElementById('tagSelect');
            select.innerHTML = '';
            data.forEach(tag => {
                const opt = document.createElement('option');
                opt.value = tag.id;
                opt.textContent = tag.name;
                select.appendChild(opt);
            });

            if (tagChoices) tagChoices.destroy();
            tagChoices = new Choices(select, {
                removeItemButton: true,
                placeholderValue: 'Оберіть теги для зручності пошуку записів',
                searchEnabled: false,
                itemSelectText: '',
            });
        });
}

function getMedications() {
    fetch(medUri)
        .then(res => res.json())
        .then(data => {
            medications = data;
            const select = document.getElementById('medicationSelect');
            select.innerHTML = '';
            data.forEach(med => {
                const opt = document.createElement('option');
                opt.value = med.id;
                opt.textContent = med.name;
                select.appendChild(opt);
            });

            if (medicationChoices) medicationChoices.destroy();
            medicationChoices = new Choices(select, {
                removeItemButton: true,
                placeholderValue: 'Оберіть ліки, які приймали сьогодні',
                searchEnabled: false,
                itemSelectText: '',
            });
        });
}

function displayRecords() {
    const container = document.getElementById('recordList');
    container.innerHTML = '';

    if (records.length === 0) {
        container.innerHTML = '<p style="text-align:center; color: #777;">Немає записів.</p>';
        return;
    }

    records.forEach(r => {
        const div = document.createElement('div');
        div.className = 'mood-item';

        const moodName = r.mood ?? 'Настрій не вказано';
        const date = r.date.split('T')[0];
        const pulse = r.pulse ?? '-';
        const sleep = r.sleepHours ?? '-';

        const info = document.createElement('div');
        info.className = 'mood-info';
        info.innerHTML = `
            <strong>${date}</strong><br/>
            🫀 Пульс: ${pulse} &nbsp;&nbsp;&nbsp; 💤 Сон: ${sleep} год<br/>
            😊 Настрій: <em>${moodName}</em>
        `;

        const tagWrapper = document.createElement('div');
        tagWrapper.className = 'badge-list';
        (r.tags ?? []).forEach(t => {
            const tag = document.createElement('span');
            tag.className = 'badge';
            tag.textContent = `🏷️ ${t.name}`;
            tagWrapper.appendChild(tag);
        });
        info.appendChild(tagWrapper);

        const medWrapper = document.createElement('div');
        medWrapper.className = 'badge-list';
        (r.medications ?? []).forEach(m => {
            const med = document.createElement('span');
            med.className = 'badge';
            med.textContent = `💊 ${m.name}`;
            medWrapper.appendChild(med);
        });
        info.appendChild(medWrapper);

        const actions = document.createElement('div');
        actions.className = 'mood-actions';

        const delBtn = document.createElement('button');
        delBtn.title = 'Видалити';
        delBtn.innerHTML = '🗑';
        delBtn.onclick = () => deleteRecord(r.id);

        actions.appendChild(delBtn);
        div.appendChild(info);
        div.appendChild(actions);
        container.appendChild(div);
    });
}

function addRecord() {
    const selectedTags = Array.from(document.getElementById('tagSelect').selectedOptions).map(o => ({
        tagId: parseInt(o.value)
    }));

    const selectedMeds = Array.from(document.getElementById('medicationSelect').selectedOptions).map(o => ({
        medicationId: parseInt(o.value)
    }));

    const record = {
        date: document.getElementById('date').value,
        pulse: +document.getElementById('pulse').value || null,
        systolicPressure: +document.getElementById('systolic').value || null,
        diastolicPressure: +document.getElementById('diastolic').value || null,
        sleepHours: +document.getElementById('sleep').value || null,
        bodyTemperature: +document.getElementById('temperature').value || null,
        waterIntakeLiters: +document.getElementById('water').value || null,
        steps: +document.getElementById('steps').value || null,
        weight: +document.getElementById('weight').value || null,
        stressLevel: document.getElementById('stress').value ? +document.getElementById('stress').value : null,
        note: document.getElementById('note').value || '',
        moodOptionId: parseInt(document.getElementById('moodOptionId').value) || null,
        tags: selectedTags,
        medications: selectedMeds
    };

    fetch(uri, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(record)
    })
    .then(res => {
        if (res.status === 409) {
            throw new Error("На цю дату вже існує запис.");
        }
        if (!res.ok) {
            return res.text().then(t => { throw new Error(t); });
        }
        return res.json();
    })
    .then(() => {
        clearForm();
        // getRecords();
    })
    .catch(err => alert("Помилка: " + err.message));
}


function deleteRecord(id) {
    if (!confirm('Ви впевнені, що хочете видалити цей запис?')) return;
    fetch(`${uri}/${id}`, { method: 'DELETE' })
        .then(() => getRecords());
}

function clearForm() {
    document.querySelector('form').reset();
    document.querySelectorAll('#stress-picker label').forEach(l => l.classList.remove('selected'));
    if (tagChoices) tagChoices.removeActiveItems();
    if (medicationChoices) medicationChoices.removeActiveItems();
    if (moodChoices) moodChoices.removeActiveItems();
}

document.addEventListener('DOMContentLoaded', () => {
    const dateInput = document.getElementById('date');
    if (!dateInput) {
        console.error("Не знайдено поле з id='date'");
        return;
    }

    fetch('/api/healthrecords')
        .then(res => res.json())
        .then(data => {
            const disabledDates = data.map(r => r.date.split('T')[0]);
            console.log("Заблоковані дати:", disabledDates);

            flatpickr(dateInput, {
                locale: 'uk',
                dateFormat: 'Y-m-d',
                disable: disabledDates,
                maxDate: 'today'
            });
        })
        .catch(err => console.error("Помилка при завантаженні записів:", err));

    document.querySelectorAll('#stress-picker label').forEach(label => {
        label.addEventListener('click', () => {
            document.querySelectorAll('#stress-picker label').forEach(l => l.classList.remove('selected'));
            label.classList.add('selected');
            document.getElementById('stress').value = label.getAttribute('data-value');
        });
    });
});


getMoods();
getTags();
getMedications();
// getRecords();

