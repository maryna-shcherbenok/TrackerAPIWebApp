document.addEventListener('DOMContentLoaded', () => {
    loadStressChart();
    loadMedicationChart();
});

async function loadStressChart() {
    const ctx = document.getElementById('stressChart').getContext('2d');
    try {
        const res = await fetch('/api/stats/stress-last-month');
        const data = await res.json();

        const labels = data.map(d => d.date || d.Date);
        const values = data.map(d => d.value || d.Value);

        new Chart(ctx, {
            type: 'line',
            data: {
                labels,
                datasets: [{
                    label: 'Рівень стресу',
                    data: values,
                    borderColor: '#7c3aed',
                    backgroundColor: '#e9d5ff',
                    tension: 0.4,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
    } catch (err) {
        console.error('Помилка завантаження стресу:', err);
    }
}

async function loadMedicationChart() {
    const ctx = document.getElementById('medChart').getContext('2d');
    try {
        const res = await fetch('/api/stats/medication-frequency');
        const data = await res.json();

        const labels = data.map(d => d.medication || d.Medication);
        const values = data.map(d => d.count || d.Count);

        new Chart(ctx, {
            type: 'bar',
            data: {
                labels,
                datasets: [{
                    label: 'Кількість прийомів',
                    data: values,
                    backgroundColor: '#a78bfa',
                    borderRadius: 6
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
    } catch (err) {
        console.error('Помилка завантаження ліків:', err);
    }
}
