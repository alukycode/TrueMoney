function handleApproveClick() {
    $('#сapproveButtonId').prop('disabled', true);
}

function handleApproveSuccess() {
    $('#сapproveButtonId').prop('disabled', false);
    $('#сapproveButtonId').hide();
    $('#anselApproveButtonId').show();
}

function handleCanselApproveClick() {
    $('#сanselApproveButtonId').prop('disabled', true);
}

function handleCanselApproveSuccess() {
    $('#сanselApproveButtonId').prop('disabled', false);
    $('#сanselOfferApprove').hide();
    $('#approveButtonId').show();
}