const uri = '/api/healthrecords';
const moodUri = '/api/moodoptions';
const tagUri = '/api/tags';
const medUri = '/api/medications';

let records = [];
let moods = [];
let tags = [];
let medications = [];

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
}

function displayRecords() {
    const container = document.getElementById('recordList');
    container.innerHTML = '';

    if (records.length === 0) {
        container.innerHTML = '<p>Немає записів.</p>';
        return;
    }

    records.forEach(r => {
        const div = document.createElement('div');
        div.className = 'mood-item';

        const moodName = r.mood ?? 'Настрій не вказано';

        const summary = document.createElement('span');
        summary.textContent = `${r.date.split('T')[0]} – пульс: ${r.pulse ?? '-'}, сон: ${r.sleepHours ?? '-'}, настрій: ${moodName}`;

        const delBtn = document.createElement('button');
        delBtn.textContent = '🗑';
        delBtn.title = 'Видалити';
        delBtn.onclick = () => deleteRecord(r.id);

        div.appendChild(summary);
        div.appendChild(delBtn);
        container.appendChild(div);
    });
}

function addMood() {
    const input = document.getElementById('newMood');
    const name = input.value.trim();

    if (!name) return alert('Введіть назву настрою.');

    fetch(moodUri, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name })
    })
        .then(res => res.json())
        .then(newMood => {
            moods.push(newMood);
            populateMoodSelect();
            document.getElementById('moodOptionId').value = newMood.id;
            input.value = '';
        })
        .catch(err => alert(err.message));
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
    }).then(res => {
        if (!res.ok) return res.text().then(t => { throw new Error(t); });
        return res.json();
    }).then(() => {
        clearForm();
        getRecords();
    }).catch(err => alert("Помилка: " + err.message));
}

function deleteRecord(id) {
    if (!confirm('Ви впевнені, що хочете видалити цей запис?')) return;
    fetch(`${uri}/${id}`, { method: 'DELETE' })
        .then(() => getRecords());
}

function clearForm() {
    document.querySelector('form').reset();
    document.querySelectorAll('#stress-picker label').forEach(l => l.classList.remove('selected'));
}

// Встановлює значення stressLevel з емодзі
document.querySelectorAll('#stress-picker label').forEach(label => {
    label.addEventListener('click', () => {
        document.querySelectorAll('#stress-picker label').forEach(l => l.classList.remove('selected'));
        label.classList.add('selected');
        document.getElementById('stress').value = label.getAttribute('data-value');
    });
});

getMoods();
getTags();
getMedications();
getRecords();

