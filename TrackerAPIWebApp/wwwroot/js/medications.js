const apiUrl = '/api/medications';

function getMedications() {
    fetch(apiUrl)
        .then(res => res.json())
        .then(data => displayMedications(data))
        .catch(err => console.error('Помилка завантаження:', err));
}

function displayMedications(medications) {
    const container = document.getElementById('medicationList');
    container.innerHTML = '';

    if (medications.length === 0) {
        container.innerHTML = '<p>Немає препаратів.</p>';
        return;
    }

    medications.forEach(med => {
        const div = document.createElement('div');
        div.className = 'mood-item';

        const name = document.createElement('span');
        name.textContent = med.name;

        const actions = document.createElement('div');

        const editBtn = document.createElement('button');
        editBtn.textContent = '✎';
        editBtn.title = 'Редагувати';
        editBtn.onclick = () => editMedication(med);

        const deleteBtn = document.createElement('button');
        deleteBtn.textContent = '🗑';
        deleteBtn.title = 'Видалити';
        deleteBtn.onclick = () => deleteMedication(med.id);

        actions.appendChild(editBtn);
        actions.appendChild(deleteBtn);

        div.appendChild(name);
        div.appendChild(actions);

        container.appendChild(div);
    });
}

function addMedication() {
    const name = document.getElementById('add-name').value.trim();
    const description = document.getElementById('add-description').value.trim();

    if (!name) return alert('Введіть назву препарату.');

    fetch(apiUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, description })
    })
        .then(res => {
            if (!res.ok) return res.text().then(t => { throw new Error(t); });
            return res.json();
        })
        .then(() => {
            document.getElementById('add-name').value = '';
            document.getElementById('add-description').value = '';
            getMedications();
        })
        .catch(err => alert('Помилка додавання: ' + err.message));
}

function editMedication(med) {
    document.getElementById('edit-id').value = med.id;
    document.getElementById('edit-name').value = med.name;
    document.getElementById('edit-description').value = med.description || '';
    document.getElementById('editMedication').classList.remove('hidden');
}

function updateMedication() {
    const id = +document.getElementById('edit-id').value;
    const name = document.getElementById('edit-name').value.trim();
    const description = document.getElementById('edit-description').value.trim();

    if (!name) return alert('Назва не може бути порожньою.');

    fetch(`${apiUrl}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ id, name, description })
    })
        .then(res => {
            if (!res.ok) return res.text().then(t => { throw new Error(t); });
            closeInput();
            getMedications();
        })
        .catch(err => alert('Помилка оновлення: ' + err.message));
}

function deleteMedication(id) {
    if (!confirm('Ви справді хочете видалити цей препарат?')) return;

    fetch(`${apiUrl}/${id}`, {
        method: 'DELETE'
    })
        .then(res => {
            if (!res.ok) return res.text().then(t => { throw new Error(t); });
            getMedications();
        })
        .catch(err => alert('Помилка видалення: ' + err.message));
}

function closeInput() {
    document.getElementById('edit-id').value = '';
    document.getElementById('edit-name').value = '';
    document.getElementById('edit-description').value = '';
    document.getElementById('editMedication').classList.add('hidden');
}

getMedications();
