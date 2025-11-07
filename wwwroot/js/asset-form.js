// This object holds the category-to-subcategory mapping.
var subcategoryData = {
    "IT Equipment": ["Laptop", "Monitor", "Printer"],
    "Furniture": ["Chair", "Table", "Cabinet"]
};

/**
 * Populates the subcategory dropdown based on the selected category.
 * @param {string} selectedCategory The value from the category dropdown.
 * @param {jQuery} $subcategoryDropdown The jQuery object for the subcategory dropdown.
 */
function updateSubcategoryDropdown(selectedCategory, $subcategoryDropdown) {
    $subcategoryDropdown.empty();

    if (selectedCategory && subcategoryData[selectedCategory]) {
        $subcategoryDropdown.append($('<option>', { value: '', text: '-- Select Subcategory --' }));
        $.each(subcategoryData[selectedCategory], function (index, value) {
            $subcategoryDropdown.append($('<option>', {
                value: value,
                text: value
            }));
        });
    } else {
        $subcategoryDropdown.append($('<option>', { value: '', text: '-- Select Category First --' }));
    }
}

// This block runs when the DOM is ready. It attaches the event handler.
$(document).ready(function () {
    $("#categoryDropdown").change(function () {
        var selectedCategory = $(this).val();
        var $subcategoryDropdown = $("#subcategoryDropdown");
        updateSubcategoryDropdown(selectedCategory, $subcategoryDropdown);
    });
});