$(function () {
    $('#modalDelete').on('show.bs.modal',
        function(event) {
        	$(this).fadeIn('fast');	
            var button = $(event.relatedTarget);
            var localId = button.attr('data-localId');

            var modal = $(this)
            modal.find('#deleteForm').attr('action', '/Locals/Delete/' + localId);
            modal.find('.modal-title').html('Delete Local');
        });

    $('#modalDeleteDish').on('show.bs.modal',
        function (event) {
            $(this).fadeIn('fast');
            var button = $(event.relatedTarget);
            var dishId = button.attr('data-dishId');
            var dishName = button.attr('data-dishName');

            var modal = $(this)
            modal.find('#deleteForm').attr('action', '/Dishes/Delete/' + dishId);
            modal.find('.modal-title').html('Delete '+ dishName);
        });
});