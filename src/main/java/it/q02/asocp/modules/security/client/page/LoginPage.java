package it.q02.asocp.modules.security.client.page;

import com.google.gwt.core.client.GWT;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.event.dom.client.KeyCodes;
import com.google.gwt.event.dom.client.KeyPressEvent;
import com.google.gwt.http.client.*;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.uibinder.client.UiHandler;
import com.google.gwt.user.client.Window;
import com.google.gwt.user.client.rpc.AsyncCallback;
import com.google.gwt.user.client.ui.HTML;
import it.q02.asocp.modules.base.client.data.RoleMap;
import it.q02.asocp.modules.base.client.helpers.UserRoles;
import it.q02.asocp.modules.security.client.rpc.CanHandleUserRoles;
import it.q02.asocp.modules.security.client.rpc.CanHandleUserRolesAsync;
import it.q02.asocp.modules.base.client.widgets.selection.SelectionWidget;
import org.gwtbootstrap3.client.ui.*;
import org.gwtbootstrap3.client.ui.constants.ValidationState;

import java.util.Collection;
import java.util.List;

import static it.q02.asocp.modules.base.client.helpers.CommonHelper.isNullOrEmpty;

/**
 * User: aleksander at  16.03.14, 19:40
 */
public class LoginPage {
    interface LoginPageUiBinder extends UiBinder<Modal, LoginPage> {
    }

    private static LoginPageUiBinder ourUiBinder = GWT.create(LoginPageUiBinder.class);

    private static final CanHandleUserRolesAsync getUserRolesService = GWT.create(CanHandleUserRoles.class);

    @UiField
    protected TextBox login;
    @UiField
    protected Form loginForm;
    @UiField
    protected Input password;
    @UiField
    protected FormGroup passwordGroup;
    @UiField
    protected FormGroup loginGroup;
    @UiField
    protected Alert alert;

    public LoginPage() {
        Modal rootElement = ourUiBinder.createAndBindUi(this);
        rootElement.show();
    }

    @UiHandler("password")
    public void onKeyPress(KeyPressEvent event) {
        if (event.getNativeEvent().getKeyCode() == KeyCodes.KEY_ENTER) {
            makeCall();
        }
    }

    @UiHandler("clear")
    public void onClear(ClickEvent event) {
        login.setText("");
        password.setFormValue("");
    }

    protected boolean validateGroup(String value, FormGroup group) {
        boolean isError = false;
        if (value.trim().isEmpty()) {
            group.setValidationState(ValidationState.ERROR);
            isError = true;
        } else {
            group.setValidationState(ValidationState.NONE);
            isError = false;
        }
        return isError;
    }

    @UiHandler("enter")
    public void onEnter(ClickEvent event) {
        boolean isError = false;
        isError |= validateGroup(login.getValue(), loginGroup);
        isError |= validateGroup(password.getFormValue(), passwordGroup);

        if (!isError) {
            alert.setVisible(false);
            alert.clear();
            makeCall();
        } else {
            alert.setVisible(true);
            alert.clear();
            alert.add(new HTML("<strong>Ошибка</strong> заполнения формы"));
        }
    }

    public void makeCall() {

        RequestBuilder requestBuilder = new RequestBuilder(RequestBuilder.POST, "../j_security_check");
        requestBuilder.setHeader("Content-type", "application/x-www-form-urlencoded;charset=utf-8");
        StringBuilder postData = new StringBuilder();
        postData.append(URL.encodeQueryString("j_character_encoding")).append("=").append(URL.encodeQueryString("UTF-8"));
        postData.append("&");
        postData.append(URL.encodeQueryString("j_username")).append("=").append(URL.encodeQueryString(login.getValue()));
        postData.append("&");
        postData.append(URL.encodeQueryString("j_password")).append("=").append(URL.encodeQueryString(password.getFormValue()));

        requestBuilder.setRequestData(postData.toString());

        requestBuilder.setCallback(new RequestCallback() {
            public void onResponseReceived(Request request, Response response) {

                if ("true".equalsIgnoreCase(response.getHeader("IS_ERROR"))) {
                    alert.setVisible(true);
                    alert.clear();
                    alert.add(new HTML("<strong>Ошибка</strong> неверный логин или пароль"));
                    passwordGroup.setValidationState(ValidationState.ERROR);
                    loginGroup.setValidationState(ValidationState.ERROR);
                    password.setFormValue("");
                    login.setFocus(true);
                } else {

                    alert.clear();

                    getUserRolesService.getRoleMaps(new AsyncCallback<List<RoleMap>>() {
                        public void onSuccess(List<RoleMap> result) {
                            if (result == null) {
                                alert.add(new HTML("<strong>Ошибка</strong> у вас недостаточно полномочий для работы"));
                                alert.setVisible(true);
                                login.setFocus(true);
                                return;
                            }

                            passwordGroup.setVisible(false);
                            loginGroup.setVisible(false);

                            if (result.size() == 1) {
                                Window.Location.replace(result.get(0).getUrl());
                                return;
                            }

                            alert.add(new SelectionWidget(result));
                            alert.setVisible(true);
                        }

                        public void onFailure(Throwable caught) {
                            GWT.log(caught.getMessage());
                        }
                    });
                }
            }

            public void onError(Request request, Throwable exception) {

            }
        });

        try {
            requestBuilder.send();
        } catch (RequestException e) {

        }
    }

}