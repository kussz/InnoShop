async function auth() {
    const token = localStorage.getItem("jwtToken");
    if (token)
        try {
            const loadingElement = document.getElementById('loading');
            const profileElement = document.getElementById('userProfile');
            const response = await fetch('http://localhost:5069/User/GetProfile', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            if (!response.ok) {
                throw new Error('Ошибка входа: ' + response.statusText);
            }
            user = await response.json();
            if (user == null)
                window.location.href = "/Account/Unauthorized"
            return user;
        }
        catch (error) { console.log(error) }
    else
        window.location.href = "/Account/Unauthorized"
        
}