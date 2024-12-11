document.querySelector('form').addEventListener('submit', function (event) {
    var amountInput = document.getElementById('amount');
    var amountError = document.getElementById('amountError');

    var amountValue = amountInput.value.replace(/₱|,/g, '').trim();
    var amountNumber = parseFloat(amountValue);

    var minAmount = 6230;

    if (isNaN(amountNumber) || amountNumber < minAmount) {
        amountError.style.display = 'block';
        amountInput.classList.add('is-invalid');
        event.preventDefault();
    } else {
        amountError.style.display = 'none';
        amountInput.classList.remove('is-invalid');
    }
});

/* Limit the value upto 24,490 pesos to avoid the putanginang conflict sa system */
document.getElementById("amount").addEventListener("input", function () {
    const minAmount = 6230;
    const maxAmount = 24490;
    const amountField = this;
    const amountError = document.getElementById("amountError");

    // Parse input value as a number
    const amountValue = parseFloat(amountField.value);

    if (isNaN(amountValue) || amountValue < minAmount) {
        amountField.style.borderColor = "red";
        amountError.style.display = "block";
        amountError.textContent = `Please enter an amount of at least ₱${minAmount.toLocaleString()}.`;
    } else if (amountValue > maxAmount) {
        amountField.style.borderColor = "red";
        amountError.style.display = "block";
        amountError.textContent = `Please enter an amount of no more than ₱${maxAmount.toLocaleString()}.`;
    } else {
        amountField.style.borderColor = "";
        amountError.style.display = "none";
    }
});