function CheckBoxValidatorDisableButton(chkId, mustBeChecked, btnId)
{
    var button = document.getElementById(btnId);
    var chkbox = document.getElementById(chkId);
    
    if (button && chkbox)
    {
        button.disabled = (chkbox.checked != mustBeChecked);
    }
}

function CheckBoxValidatorEvaluateIsValid(val)
{
    var control = document.getElementById(val.controltovalidate);
    var mustBeChecked = Boolean(val.mustBeChecked == 'true');

    return control.checked == mustBeChecked;
}

function CheckBoxListValidatorEvaluateIsValid(val)
{
    var control = document.getElementById(val.controltovalidate);
    var minimumNumberOfSelectedCheckBoxes = parseInt(val.minimumNumberOfSelectedCheckBoxes);

    var selectedItemCount = 0;
    var liIndex = 0;
    var currentListItem = document.getElementById(control.id + '_' + liIndex.toString());
    while (currentListItem != null)
    {
        if (currentListItem.checked) selectedItemCount++;
        liIndex++;
        currentListItem = document.getElementById(control.id + '_' + liIndex.toString());
    }
    
    return selectedItemCount >= minimumNumberOfSelectedCheckBoxes;
}
