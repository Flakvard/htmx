htmx.onLoad(function (content) {
  var sortables = content.querySelectorAll(".sortable");
  for (var i = 0; i < sortables.length; i++) {
    var sortable = sortables[i];
    var sortableInstance = new Sortable(sortable, {
      swapThreshold: 1,
      animation: 150,
      ghostClass: "blue-background-class",

      // Make the `.htmx-indicator` unsortable
      filter: ".htmx-indicator",
      onMove: function (evt) {
        return evt.related.className.indexOf("htmx-indicator") === -1;
      },

      // Disable sorting on the `end` event
      onEnd: function (evt) {
        var currentIds = Array.from(sortable.querySelectorAll("[data-id]")).map(
          (el) => el.getAttribute("data-id")
        );
        var missingIds = originalIds.filter((id) => !currentIds.includes(id));

        if (missingIds.length > 0) {
          alert("Error: Some rows are missing. Please try again.");
          this.option("disabled", false);
        } else {
          this.option("disabled", true);
          // Trigger the form submission
          htmx.trigger(sortable, "submit");
        }
      },
    });

    // Re-enable sorting on the `htmx:afterSwap` event
    sortable.addEventListener("htmx:afterSwap", function () {
      sortableInstance.option("disabled", false);
    });
  }
});
