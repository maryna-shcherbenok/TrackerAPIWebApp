const uri = 'api/tags';
let tags = [];

function getTags() {
    fetch(uri)
        .then(res => res.json())
        .then(data => {
            tags = data;
            displayTags();
        });
}

function displayTags() {
    const container = document.getElementById('tagList');
    container.innerHTML = '';

    if (tags.length === 0) {
        container.innerHTML = '<p>Немає жодного тегу.</p>';
        return;
    }

    tags.forEach(tag => {
        const div = document.createElement('div');
        div.className = 'mood-item';

        const name = document.createElement('span');
        name.textContent = tag.name;

        const actions = document.createElement('div');

        const editBtn = document.createElement('button');
        editBtn.textContent = '✎';
        editBtn.title = 'Редагувати';
        editBtn.onclick = () => displayEditForm(tag.id);

        const deleteBtn = document.createElement('button');
        deleteBtn.textContent = '🗑';
        deleteBtn.title = 'Видалити';
        deleteBtn.onclick = () => deleteTag(tag.id);

        actions.appendChild(editBtn);
        actions.appendChild(deleteBtn);

        div.appendChild(name);
        div.appendChild(actions);

        container.appendChild(div);
    });
}

function addTag() {
    const name = document.getElementById('add-name').value.trim();
    if (!name) return;

    fetch(uri, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name })
    }).then(() => {
        document.getElementById('add-name').value = '';
        getTags();
    });
}

function deleteTag(id) {
    if (!confirm('Ви впевнені, що хочете видалити цей тег?')) return;
    fetch(`${uri}/${id}`, { method: 'DELETE' })
        .then(response => {
            if (response.ok) {
                getTags();
            } else {
                alert("Неможливо видалити тег, який використовується.");
            }
        });
}

function displayEditForm(id) {
    const tag = tags.find(t => t.id === id);
    document.getElementById('edit-id').value = tag.id;
    document.getElementById('edit-name').value = tag.name;
    document.getElementById('editTag').classList.remove('hidden');
}

function updateTag() {
    const id = document.getElementById('edit-id').value;
    const name = document.getElementById('edit-name').value.trim();

    fetch(`${uri}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ id: parseInt(id), name })
    }).then(() => {
        closeInput();
        getTags();
    });
}

function closeInput() {
    document.getElementById('editTag').classList.add('hidden');
}

getTags();
