document.getElementById('formButton').addEventListener('click', async function (e) {
    e.preventDefault(); // Предотвращаем стандартное поведение формы
    await funch(tableName,methods);
});

async function funch() {
    const url = window.location.href;
    const urlParts = url.split('/'); // Разделяем URL по '/'
    var id = urlParts[urlParts.length - 1];// Предполагаем, что id — это последний элемент
    if (!Number.isInteger(+id))
        id = 0;
    const name = document.getElementById('name').value;
    try {
        // Выполняем POST-запрос
        const response = await fetch(`${host}/${tableName}/${methods}`, {
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
            window.location.href =`/${tableName}`
    } catch (error) {

    }
}