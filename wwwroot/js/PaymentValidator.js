/* Limit the value upto 24,490 pesos to avoid the putanginang conflict sa system */
document.getElementById("amount").addEventListener("input", function () {
    const minAmount = 6230;
    const maxAmount = 24920;
    const amountField = this;
    const amountError = document.getElementById("amountError");

    // Parse lang naten yung input as a number
    const amountValue = parseFloat(amountField.value);

    // Check if the amount is within the accepted range
    if (isNaN(amountValue) || amountValue < minAmount) {
        amountField.style.borderColor = "red";
        amountError.style.display = "block";
        amountError.textContent = `Please enter an amount of at least ₱${minAmount.toLocaleString()}.`;
    } else if (amountValue > maxAmount) {
        amountField.style.borderColor = "red";
        amountError.style.display = "block";
        amountError.textContent = `Please enter an amount of no more than ₱${maxAmount.toLocaleString()}.`;
    } else {
        amountField.style.borderColor = ""; // Reset border color
        amountError.style.display = "none"; // Hide error message
    }
});

// Prevent form submission if the amount is out of range
document.querySelector("form").addEventListener("submit", function (event) {
    const amountField = document.getElementById("amount");
    const amountValue = parseFloat(amountField.value);

    if (isNaN(amountValue) || amountValue < 6230 || amountValue > 24920) {
        event.preventDefault(); // Stop form submission
        alert("Please enter a valid amount between ₱6,230 and ₱24,920.");
    }
});