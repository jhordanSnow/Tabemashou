$(function () {
    $('#modalDelete').on('show.bs.modal',
        function(event) {
        	$(this).fadeIn('fast');	
            var button = $(event.relatedTarget);
            var restId = button.attr('data-restaurantId');
            var restName = button.attr('data-restaurantName');

            var modal = $(this)
            modal.find('#deleteForm').attr('action', '/Restaurants/Delete/' + restId);
            modal.find('.modal-title').html('Delete ' + restName);
        });

});