MVCxClientGlobalEvents.AddControlsInitializedEventHandler(adjustNavMenu);
$(document).ready(function() {
    $(window).resize(adjustNavMenu);
    $(window).scroll(function() {
        toogleFixedTopPanel();
        toogleBackToTopButton();
    });
    $("#btnBackToTop").click(function(event) {
        event.preventDefault();
        $("html, body").animate({ scrollTop: 0 }, '500');
        return false;
    });
});

function toogleFixedTopPanel() {
    if(!isTopPanelExists())
        return;

    if($(document).scrollTop() > 100 && !TopPanel.IsExpandable()) {
        $('header').addClass('shrink');
        $('body').css('padding-top', $('header').outerHeight());
    } else {
        $('header').removeClass('shrink');
        $('body').css('padding-top', 0);
    }
}
function toogleBackToTopButton() {
    var offset = 200;
    if($(this).scrollTop() > offset)
        $(".btnBackToTop").removeClass("hidden");
    else
        $(".btnBackToTop").addClass("hidden");
}
function adjustNavMenu() {
    if(!isTopPanelExists() || !window.NavMenu || !isASPxClientControl(NavMenu))
        return;

    var orientation = TopPanel.IsExpandable() ? 'Vertical' : 'Horizontal';
    if(orientation !== NavMenu.GetOrientation())
        NavMenu.SetOrientation(orientation);
}

function isTopPanelExists() {
    return window.TopPanel && isASPxClientControl(TopPanel);
}
function isASPxClientControl(obj) {
    return obj instanceof ASPxClientControlBase;
}

const sTokenName = "__RequestVerificationToken";

// Возвращает проверочный токен
function GetVerificationToken() {
	const vContainer = document.getElementById("FormLogOff");

	if (vContainer !== null) {
		const sToken = $("input[name=\"" + sTokenName + "\"]", vContainer).val();

		if (sToken !== null)
			return sToken;
	}
	return "";
}

// Открыть печатную форму
//var aParams = { 'emailId' : emailId, 'age': age };
function OpenPrintWindow(aParams, sFormAction)
{
	var vForm = document.createElement("form");
	var sTarget = "PrintFormWindow";
	aParams[sTokenName] = GetVerificationToken();

	for (var sParamNme in aParams)
	{
		if (aParams.hasOwnProperty(sParamNme))
		{
			var vInput = document.createElement("input");
			vInput.type = "hidden";
			vInput.name = sParamNme;
			vInput.value = aParams[sParamNme];
			vForm.appendChild(vInput);

			if(sTokenName !== sParamNme)
				sTarget += "_" + aParams[sParamNme];
		}
	}

	vForm.setAttribute("method", "post");
	vForm.setAttribute("action", sFormAction);
	vForm.setAttribute("target", sTarget);
	vForm.setAttribute("style", "display: none");

	var win = window.open("about:blank", sTarget);

	if (win != null)
		win.document.title = "Загрузка ...";

	document.body.appendChild(vForm);

	//window.open("about:blank", sTarget);
	vForm.submit();
	document.body.removeChild(vForm);
}

// Форматирование даты
function FormateDate(dDate, sDateDelim = ".", iType = 1, bAddTime = false) {
	const iMonth = (dDate.getMonth() + 1).toString();
	const iDay = dDate.getDate().toString();
	var sResult;

	if (iType === 1)
		sResult = ((iDay.length === 1) ? ("0" + iDay) : iDay) + sDateDelim + ((iMonth.length === 1) ? ("0" + iMonth) : iMonth) + sDateDelim + dDate.getFullYear();
	else
		sResult = dDate.getFullYear() + sDateDelim + ((iMonth.length === 1) ? ("0" + iMonth) : iMonth) + sDateDelim + ((iDay.length === 1) ? ("0" + iDay) : iDay);

	if (bAddTime) {
		var iHour = dDate.getHours().toString(),
			iMinute = dDate.getMinutes().toString(),
			iSecond = dDate.getSeconds().toString();
		sResult += " " + ((iHour.length === 1) ? ("0" + iHour) : iHour) + ":" + ((iMinute.length === 1) ? ("0" + iMinute) : iMinute) + ":" + ((iSecond.length === 1) ? ("0" + iSecond) : iSecond);
	}

	return sResult;
}