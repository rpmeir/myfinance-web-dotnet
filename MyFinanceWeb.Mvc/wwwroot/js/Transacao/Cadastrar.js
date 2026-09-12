(function () {
    const form = document.getElementById('transacaoForm');
    const visibleValue = document.getElementById('ValorDisplay');
    const numericValue = document.getElementById('Valor');

    if (!form || !visibleValue || !numericValue) {
        return;
    }

    function formatValue(value) {
        const digits = value.replace(/\D/g, '');
        const cents = digits.padStart(3, '0');
        const integerPart = cents.slice(0, -2).replace(/^0+(?=\d)/, '') || '0';
        const decimalPart = cents.slice(-2);
        const integerGroups = [];
        for (let end = integerPart.length; end > 0; end -= 3) {
            integerGroups.unshift(integerPart.slice(Math.max(0, end - 3), end));
        }
        const formattedInteger = integerGroups.join('.');

        return formattedInteger + ',' + decimalPart;
    }

    function updateValue() {
        visibleValue.value = formatValue(visibleValue.value);
        numericValue.value = visibleValue.value
            .replaceAll('.', '')
            .replaceAll(',', '.');
    }

    visibleValue.addEventListener('input', updateValue);
    visibleValue.addEventListener('blur', updateValue);
    form.addEventListener('submit', updateValue);

    updateValue();
})();
