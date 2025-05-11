const uri = 'api/moodoptions';
const recordUri = 'api/healthrecords';
let moods = [];

function getMoods() {
    fetch(uri)
        .then(res => res.json())
        .then(data => {
            moods = data;
            displayMoods();
            populateMoodSelect();
        });
}

function displayMoods() {
    const container = document.getElementById('moodList');
    if (!container) return;

    container.innerHTML = '';
    if (moods.length === 0) {
        container.innerHTML = '<p>Настроїв поки немає.</p>';
        return;
    }

    moods.forEach(mood => {
        const div = document.createElement('div');
        div.className = 'mood-item';

        const name = document.createElement('span');
        name.textContent = mood.name;

        const actions = document.createElement('div');

        const editBtn = document.createElement('button');
        editBtn.textContent = '✎';
        editBtn.title = 'Редагувати';
        editBtn.onclick = () => displayEditForm(mood.id);

        const deleteBtn = document.createElement('button');
        deleteBtn.textContent = '🗑';
        deleteBtn.title = 'Видалити';
        deleteBtn.onclick = () => deleteMood(mood.id);

        actions.appendChild(editBtn);
        actions.appendChild(deleteBtn);

        div.appendChild(name);
        div.appendChild(actions);

        container.appendChild(div);
    });
}

function addMood() {
    const name = document.getElementById('add-name').value.trim();
    if (!name) return;

    fetch(uri, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name })
    }).then(() => {
        document.getElementById('add-name').value = '';
        getMoods();
    });
}

function deleteMood(id) {
    if (!confirm('Ви впевнені, що хочете видалити цей настрій?')) return;
    fetch(`${uri}/${id}`, { method: 'DELETE' })
        .then(() => getMoods());
}

function displayEditForm(id) {
    const mood = moods.find(m => m.id === id);
    document.getElementById('edit-id').value = mood.id;
    document.getElementById('edit-name').value = mood.name;
    document.getElementById('editMood').classList.remove('hidden');
}

function updateMood() {
    const id = document.getElementById('edit-id').value;
    const name = document.getElementById('edit-name').value.trim();

    fetch(`${uri}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ id: parseInt(id), name })
    }).then(() => {
        closeInput();
        getMoods();
    });
}

function closeInput() {
    document.getElementById('editMood').classList.add('hidden');
}

function populateMoodSelect() {
    const select = document.getElementById('mood-select');
    if (!select) return;

    select.innerHTML = '';
    moods.forEach(m => {
        const opt = document.createElement('option');
        opt.value = m.id;
        opt.textContent = m.name;
        select.appendChild(opt);
    });
}

function submitMoodRecord() {
    const select = document.getElementById('mood-select');
    const note = document.getElementById('mood-note');
    if (!select || !note) return;

    const moodOptionId = parseInt(select.value);
    const userId = 1;
    const date = new Date().toISOString();

    const record = {
        date,
        moodOptionId,
        note: note.value,
        userId
    };

    fetch(recordUri, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(record)
    })
        .then(response => {
            if (response.ok) {
                note.value = '';
                loadMoodHistory();
            } else {
                alert('Не вдалося зберегти настрій.');
            }
        });
}

function loadMoodHistory() {
    const body = document.getElementById('mood-history-body');
    if (!body) return;

    const today = new Date();
    const weekAgo = new Date();
    weekAgo.setDate(today.getDate() - 7);

    fetch(`${recordUri}?startDate=${weekAgo.toISOString()}&endDate=${today.toISOString()}`)
        .then(res => res.json())
        .then(data => {
            body.innerHTML = '';
            data.filter(r => r.moodOption).forEach(r => {
                const tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${new Date(r.date).toLocaleDateString()}</td>
                    <td>${r.moodOption.name}</td>
                    <td>${r.note || ''}</td>
                `;
                body.appendChild(tr);
            });
        });
}

getMoods();
loadMoodHistory();
