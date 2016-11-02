function handleApproveClick(offerId) {
    $('#approveButtonId' + offerId).prop('disabled', true);
}

function handleApproveSuccess(offerId) {
    $('#approveButtonId' + offerId).prop('disabled', false);
    $('#approveButtonId' + offerId).hide();
    $('#cancelApprovalButtonId' + offerId).show();
}

function handleCancelApproveClick(offerId) {
    $('#cancelApprovalButtonId' + offerId).prop('disabled', true);
}

function handleCancelApproveSuccess(offerId) {
    $('#cancelApprovalButtonId' + offerId).prop('disabled', false);
    $('#cancelApprovalButtonId' + offerId).hide();
    $('#approveButtonId' + offerId).show();
}