getUser()
async function getUser() {
    const token = localStorage.getItem("jwtToken");
        try {
            const loadingElement = document.getElementById('loading');
            const profileElement = document.getElementById('userProfile');
        const response = await fetch(`${userHost}/User/GetProfile`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
        if (!response.ok) {
            throw new Error('Ошибка входа: ' + response.statusText);
        }
        user = await response.json();
            document.getElementById('userName').innerText = user.userName || 'N/A';
        document.getElementById('userTypeName').innerText = user.userType?.name || 'N/A';
            document.getElementById('localityName').innerText = user.locality?.name || 'N/A';
        document.getElementById('email').innerText = user.email || 'N/A';
        loadingElement.style.display = 'none';
        profileElement.style.display = 'block';
    }
        catch (error) { console.log(error) }
}