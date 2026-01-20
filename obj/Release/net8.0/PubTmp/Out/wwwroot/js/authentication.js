

window.addEventListener('load', () => {

    document.getElementById("createUserForm").addEventListener('submit', (e) => {
        validatePassword(e);
    });
});

async function validatePassword(e)
{
    //Find the 2 input fields of our create user form and pull the values (text) from each one
    var password = document.getElementById("createUserPassword").value;
    var confirmation = document.getElementById("createUserConfirmation").value;

    //Check if our fields match
    if (password !== confirmation)
    {
        //If not, prevent the event from doing its default behaviour => submitting the form
        e.preventDefault();

        var label = document.getElementById("userFormMessage");
        label.innerHTML = "Password and Confirmation do not match!"

        return;
    }
}

