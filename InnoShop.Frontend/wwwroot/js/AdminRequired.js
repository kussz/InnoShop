
token = localStorage.getItem("jwtToken");
if (token == null)
    window.location.href = "/Account/Unauthorized"
else if(getRoleFromToken(token) != "Admin")
    window.location.href ="/Account/Forbidden"
