"use strict";

// если ошибка ajax form
function OnFailure(xhr, status, error) {
	console.log(xhr.responseText);
	alert(error + "\n" + status + "\n" + " see console log");
}

const sTokenName = "__RequestVerificationToken";

function GetVerificationToken() {
	const vContainer = document.getElementById("FormLogOff");

	if (vContainer !== null) {
		const sToken = $("input[name=\"" + sTokenName + "\"]", vContainer).val();

		if (sToken !== null)
			return sToken;
	}
	return "";
}

function WebDocumentViewer_CustomizeParameterEditors(s, e) {
	if (e.parameter.name === "sMemo" || e.parameter.name === "sBase") {
		e.info.editor = { header: "custom-textarea" };
	}
}

function WebDocumentViewer_ParametersSubmitted(s, e, iOperationId, iPrintFormId, sUrl) {
	$.ajax({
		type: "POST",
		data: { sReportParams: JSON.stringify(e.Parameters), iOperationId: iOperationId, iPrintFormId: iPrintFormId, __RequestVerificationToken: GetVerificationToken() },
		url: sUrl,
		error: OnFailure
	});
}

function WebDocumentViewer_Init(s, e) {
	const vPreview = s.GetPreviewModel();

	if (vPreview) {
		vPreview.tabPanel.collapsed(false);
		vPreview.parametersModel.tabInfo.active(true);
		vPreview.parametersModel.submit();
	}
}

function OnInitResizeHeight(s, e) {
	AdjustHeight(s, e);
	ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
		AdjustHeight(s, e);
	});
}

function AdjustHeight(s, e) {
	var iDeltaHeight = 20;
	var iDeltaWidth = 20;

	//switch (s.name) {
	//	case "gvMasterOrders":
	//		iDeltaHeight = 160;
	//		break;
	//	case "gvOrderRequests":
	//		iDeltaHeight = 360;
	//		break;
	//}

	s.SetWidth(document.documentElement.clientWidth - iDeltaWidth);
	s.SetHeight(document.documentElement.clientHeight - iDeltaHeight);
}