function bindButton(buttonId, action) {
    document.getElementById(buttonId).addEventListener("click", async function (e) {
        e.preventDefault();

        try {
            const response = await fetch(`${productHost}/Product/${action}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${localStorage.getItem("jwtToken")}`
                    // Добавьте другие заголовки, если необходимо, например, авторизацию
                },
                body: id
            });

            if (response.ok) {
                location.reload();
            } else {
                const errorResponse = await response.json();
            }
        } catch (error) {
            console.error("Ошибка:", error);
            // Обработка ошибок во время запроса
        }
    });
}