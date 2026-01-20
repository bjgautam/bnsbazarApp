
window.addEventListener('load', () => {
    //Find all the components with the add-item-button class on them
    //These will be returned as a Node List
    let addButtons = document.querySelectorAll(".add-item-button");

    //Loop through the node list to process each item.
    addButtons.forEach((item) =>
    {
        //Retrieve the value field's content from each item
        let productId = parseInt(item.getAttribute("value"))
        //Add an event listener to each one that will run the 
        //add to cart method when they are clicked
        item.addEventListener('click',()=> addItemToCart(productId))
    })
});

async function addItemToCart(productId)
{
    //Send a POST request to the cart controller to run the add to cart endpoint
    let result = await fetch("/ShoppingCart/AddToCart?productId=" + productId,
        {
            method:"POST"
        });

    if (result.status == 401)
    {
        location.href = "/Authentication/Login"
    }
    else if (result.status != 200)
    {
        alert("Something Went Wrong!");
    }

    showShoppingCart();
}