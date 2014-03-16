package it.q02.asocp.modules.security.client.page;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.event.dom.client.KeyPressEvent;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.uibinder.client.UiHandler;
import com.google.gwt.user.client.Window;
import org.gwtbootstrap3.client.ui.Form;
import org.gwtbootstrap3.client.ui.Input;
import org.gwtbootstrap3.client.ui.Modal;
import org.gwtbootstrap3.client.ui.TextBox;

/**
 * User: aleksander at  16.03.14, 19:40
 */
public class LoginPage {
    interface LoginPageUiBinder extends UiBinder<Modal, LoginPage> {
    }

    private static LoginPageUiBinder ourUiBinder = GWT.create(LoginPageUiBinder.class);
    @UiField
    protected TextBox login;
    @UiField
    protected Form loginForm;
    @UiField
    protected Input password;

    public LoginPage() {
        Modal rootElement = ourUiBinder.createAndBindUi(this);
        rootElement.show();
    }

    @UiHandler("password")
    public void onKeyPress(KeyPressEvent event){
        if(event.getCharCode() == '\n'){
            Window.alert("EnterPress");
        }
    }

    @UiHandler("clear")
    public void onClear(ClickEvent event){
        Window.alert("clear");
    }

    @UiHandler("enter")
    public void onEnter(ClickEvent event){
        Window.alert("enter");
    }
}