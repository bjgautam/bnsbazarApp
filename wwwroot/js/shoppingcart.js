

window.addEventListener('load', () =>
{
    document.getElementById("btnCart").addEventListener("click", () => showShoppingCart());
})

async function showShoppingCart()
{
    var result = await fetch("/ShoppingCart/Index");

    if (result.status == 401)
    {
        location.href = "/Authentication/Login";
    }
    else if (result.status != 200)
    {
        alert("Something went wrong! \nError: " + result.statusText)
        return;
    }

    //gets the body from the response - this will be our partial view code
    var htmlResult = await result.text();
    document.getElementById("shoppingCartBody").innerHTML = htmlResult;

    disableLineItemForms();
    setupQuantityButtons();
    setupRemoveButtons();
    setupCheckoutAndCancelButtons()

    recalculateCartTotal();

    //Uses Jquery to find the shopping cart using its ID and run the 
    //offcanvas version of the show command.
    $("#shoppingCart").offcanvas("show");
}

async function disableLineItemForms()
{
    //Find all the fom components with the cart-qty-toggle class on them
    let itemQtyForms = document.querySelectorAll(".cart-qty-toggle")
    //Loop through all the forms
    itemQtyForms.forEach((form) =>
    {
        //Add a listener that triggers whenever they try to submit - whenever a button
        //insode the form is pressed
        form.addEventListener("submit", (e) =>
        {
            //Cancel the submission event from happening.
            e.preventDefault();
        });
    })
}

async function setupQuantityButtons()
{
    //Find all the buttons with the btn-minus class on them
    let minusButtons = document.querySelectorAll(".btn-minus");

    minusButtons.forEach((button) =>
    {
        //Add a listener to each button to run the change quanity method when clicked
        //and pass it the event data as well as the amount we want to change it by.
        button.addEventListener("click", (e) => changeQuantity(e, -1));
    })

    //Find all the buttons with the btn-plus class on them
    let plusButtons = document.querySelectorAll(".btn-plus");

    plusButtons.forEach((button) => {
        //Add a listener to each button to run the change quanity method when clicked
        //and pass it the event data as well as the amount we want to change it by.
        button.addEventListener("click", (e) => changeQuantity(e, 1));
    })
}

async function changeQuantity(e, amount)
{
    //Go to the target(component) of the event and find its form before finding the
    //input field (a hidden field we built earlier) and grab its value.
    let cartItemId = parseInt(e.target.form.querySelector("input").value);
    //Repeat a similar process to get the qty text field frombwteen the + and - buttons 
    //in the item
    let qty = parseInt(e.target.form.querySelector(".qty-text").innerText);
    //Adjust the quantity by the provided amount then put it back into the qty field
    qty += amount;
    //If the quantity woud drop below 1 (meaning there are none), remove it instead.
    if (qty < 1)
    {
        removeItem(e, cartItemId);
        return;
    }
    e.target.form.querySelector(".qty-text").innerText = qty;

    recalculateLineTotal(e, qty);
    recalculateCartTotal();

    updateQuantityInDatabase(qty, cartItemId);
}

async function updateQuantityInDatabase(qty, cartItemId)
{
    //Create a javascript object with the details of the item we want to update
    let updatedItem =
    {
        Id: cartItemId,
        Quantity: qty
    };
    //Send a fetch request to perform the update and pass it the JS object.
    let response = await fetch("/ShoppingCart/UpdateItemQuantity",
        {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body:JSON.stringify(updatedItem)
        }); 
    //Popup an error if something goes wrong.
    if (response.status != 200)
    {
        alert("Something went wrong!")
    }
}

async function setupRemoveButtons()
{
    //Find all the remove buttons by using their class name
    let removeButtons = document.querySelectorAll(".remove-button");

    removeButtons.forEach((button) =>
    {
        //Extract the item ID from where we hid it in the value field of each button
        let itemId = parseInt(button.getAttribute("value"));
        //Add a listener for when the button is clicked that passes the id and the 
        //event details into the remove method.
        button.addEventListener('click', (e) => removeItem(e, itemId));
    });
}
async function removeItem(e, itemId)
{
    let response = await fetch("/ShoppingCart/RemoveFromCart?Id=" + itemId,
        {
            method: "DELETE"
        });

    if (response.status != 200)
    {
        alert("Something went wrong!");
        return;
    }
    //Find the first/closest parent object with the card class on it. This shoudl be 
    //the card the line item is in.
    let parent = e.target.closest(".card");
    //Remove the card from the shopping cart popup.
    parent.remove();

    recalculateCartTotal();
}

async function recalculateCartTotal()
{
    //Get all the line items in the cart
    let lineItem = document.querySelectorAll(".line-total");
    //Cretae a vriable to add all the values to
    let total = 0.00;

    lineItem.forEach((item) =>
    {
        //Work out the price for each line based on the price and quantity
        let linePrice = parseFloat(item.innerHTML.replace("$", ""));
        total += linePrice;
    })
    //If we have not items left in the cart, remove the cancel and checkout buttons
    if (lineItem.length == 0)
    {
        document.querySelector("#btnCheckout").remove();
        document.querySelector("#btnCancel").remove();
    }

    //Update the total price field in the summary section.
    document.getElementById("cartTotal").innerHTML = "$" + total.toFixed(2) +
                                                     " <strong>.inc GST</strong>";
}

async function recalculateLineTotal(e, qty)
{
    //Find the line toal value for the current item
    let lineItem = e.target.form.querySelector(".line-total");
    //Get the value property (single item price) form the value field in the item
    let unitPrice = parseFloat(lineItem.getAttribute("value"));
    //Calculate the total price, using the Number() constructor to avoid floating point
    //calculation errors
    let totalPrice = Number(unitPrice * qty);
    //Update the line price to the new total.
    lineItem.innerText = "$" + totalPrice.toFixed(2);
}

async function setupCheckoutAndCancelButtons()
{
    //Find the button with the btnCheckout class on it
    let checkoutButton = document.querySelector("#btnCheckout");
    //If the button was found
    if (checkoutButton != null) {
        //Add an event listener to run the finalise method when pressed.
        checkoutButton.addEventListener("click", (e)=> finaliseCart(e))
    }

    //Find the button with the btnCancel class on it
    let cancelButton = document.querySelector("#btnCancel");
    //If the button was found
    if (cancelButton != null) {
        //Add an event listener to run the cancel method when pressed.
        cancelButton.addEventListener("click", (e) => cancelCart(e))
    }
}

async function finaliseCart(e)
{
    //Popup a message box with a set of yes/no options and run the IF 
    //statement if the user presses the yes button.
    if (confirm("Proceed with payment?") == true)
    {
        //Get the Cart ID from where we hid it in the button
        let cartId = parseInt(e.target.getAttribute("value"));
        //Send a fetch request to run the finalise cart method and include the 
        //cart ID as a query parameter.
        let result = await fetch("/ShoppingCart/FinaliseCart?id=" + cartId,
            {
                method: "PUT"
            });
        //Check the response code to see if the request was successful or not.
        if (result.status != 200)
        {
            alert("Something went wrong!");
        }
        else
        {
            //If it was close the cart and popup a confirmation message
            $('#shoppingCart').offcanvas("hide");
            alert("Cart Finalised. Thank you for shopping with us.");
        }
    }
}

async function cancelCart(e) {
    //Popup a message box with a set of yes/no options and run the IF 
    //statement if the user presses the yes button.
    if (confirm("Cancel Cart?") == true) {
        //Get the Cart ID from where we hid it in the button
        let cartId = parseInt(e.target.getAttribute("value"));
        //Send a fetch request to run the cancel cart method and include the 
        //cart ID as a query parameter.
        let result = await fetch("/ShoppingCart/CancelCart?id=" + cartId,
            {
                method: "DELETE"
            });
        //Check the response code to see if the request was successful or not.
        if (result.status != 200) {
            alert("Something went wrong!");
        }
        else {
            //If it was close the cart and popup a confirmation message
            $('#shoppingCart').offcanvas("hide");
            alert("Cart Cancelled.see you soon");
        }
    }
}