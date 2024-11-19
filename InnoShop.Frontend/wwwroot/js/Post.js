document.getElementById('formButton').addEventListener('click', async function (e) {
    e.preventDefault(); // Предотвращаем стандартное поведение формы
    await funch();
});

async function funch() {
    const url = window.location.href;
    const urlParts = url.split('/'); // Разделяем URL по '/'
    const id = urlParts[urlParts.length - 1]; // Предполагаем, что id — это последний элемент
    const name = document.getElementById('name').value;
    try {
        // Выполняем POST-запрос
        const response = await fetch('http://localhost:5036/Locality/Details', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Id: id,
                Name: name
            })
        })
        if (!response.ok) {
            throw new Error('Ошибка входа: ' + response.statusText);
        }
            window.location.href = "/Locality"
    } catch (error) {

    }
}