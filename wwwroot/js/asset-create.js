$(document).ready(function () {
    // Data structure to hold the subcategories for each category
    var subcategories = {
        "IT Equipment": ["Laptop", "Monitor", "Printer"],
        "Furniture": ["Chair", "Table", "Cabinet"]
    };

    // Event handler for when the category dropdown value changes
    $("#categoryDropdown").change(function () {
        var selectedCategory = $(this).val();
        var $subcategoryDropdown = $("#subcategoryDropdown");

        // Clear previous options
        $subcategoryDropdown.empty();

        // If a category is selected, populate the subcategory dropdown
        if (selectedCategory && subcategories[selectedCategory]) {
            // Add a default option
            $subcategoryDropdown.append($('<option>', {
                value: '',
                text: '-- Select Subcategory --'
            }));

            // Add the subcategories for the selected category
            $.each(subcategories[selectedCategory], function (index, value) {
                $subcategoryDropdown.append($('<option>', {
                    value: value,
                    text: value
                }));
            });
        } else {
            // If no category is selected, show the default message
            $subcategoryDropdown.append($('<option>', {
                value: '',
                text: '-- Select Category First --'
            }));
        }
    });
});