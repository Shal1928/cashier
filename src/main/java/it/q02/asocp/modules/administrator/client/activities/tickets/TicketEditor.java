package it.q02.asocp.modules.administrator.client.activities.tickets;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.event.logical.shared.ValueChangeHandler;
import com.google.gwt.event.shared.GwtEvent;
import com.google.gwt.event.shared.HandlerRegistration;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.uibinder.client.UiHandler;
import com.google.gwt.user.client.ui.HasValue;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.base.client.data.TicketRoll;
import it.q02.asocp.modules.base.client.ui.EditorStateCallback;
import org.gwtbootstrap3.client.ui.*;

import java.util.Date;
import java.util.LinkedList;
import java.util.List;

/**
 * User: aleksander at  24.03.14, 17:27
 */
public class TicketEditor implements IsWidget   {
    private final Panel rootPanel;
    private TicketRoll editableObject;
    private EditorStateCallback<TicketRoll> stateCallBack;
    private EditorStateCallback.MessageNotifer notifiter = new EditorStateCallback.MessageNotifer() {
        private List<String> messages = new LinkedList<>();
        @Override
        public void addMessage(String message) {
            messages.add(message);
        }

        @Override
        public void showMessages() {
            showErrors(messages);
        }

        @Override
        public void clear() {

        }
    };

    @Override
    public Widget asWidget() {
        return rootPanel;
    }

    public void setValue(TicketRoll ticketRoll,EditorStateCallback<TicketRoll> callBack) {
        this.stateCallBack = callBack;
        formClearWith(ticketRoll);
        changeState(ticketRoll!=null);
        this.editableObject = ticketRoll;
        if(stateCallBack!=null){
            stateCallBack.onBeginEdit(ticketRoll);
        }
    }

    private void changeState(boolean newState) {
        ticketBarcode.setEnabled(newState);
        ticketColor.setEnabled(newState);
        ticketCount.setEnabled(newState);
        buttonGroup.setVisible(newState);
    }

    private void formClearWith(TicketRoll ticketRoll) {
        showErrors(null);
        if(ticketRoll != null){
            ticketColor.setText(ticketRoll.getColor());
            ticketBarcode.setText(ticketRoll.getFirstTicketNumber());
            ticketCount.setText(Long.toString(ticketRoll.getTicketCount()));
        }else{
            ticketColor.setText("");
            ticketBarcode.setText("");
            ticketCount.setText("");
        }
        ticketBarcode.setEnabled(false);
        ticketColor.setEnabled(false);
        buttonGroup.setVisible(false);
    }



    @UiField
    TextBox ticketBarcode;
    @UiField
    TextBox ticketColor;
    @UiField
    PanelFooter buttonGroup;
    @UiField
    TextBox ticketCount;
    @UiField
    Alert errorMessage;

    public TicketEditor() {
        rootPanel = ourUiBinder.createAndBindUi(this);
    }

    @UiHandler("saveButton")
    public void onCommit(ClickEvent event){
        List<String> errorMessages = new LinkedList<String>();
        String barcodeValue = ticketBarcode.getText().trim();
        if(barcodeValue.isEmpty()){
            errorMessages.add("Штрих-код не может быть пустым!");
        }
        String colorValue = ticketColor.getText().trim();
        if(colorValue.isEmpty()){
            errorMessages.add("Цвет не может быть пустым");
        }
        String ticketCountValue = ticketCount.getText().trim();
        if(ticketCountValue.isEmpty() || !ticketCountValue.matches("\\d+")){
            errorMessages.add("Количество билетов должно быть числом!");
        }

        showErrors(errorMessages);
        if(errorMessages.isEmpty()){
            editableObject.setColor(colorValue);
            editableObject.setTicketCount(Long.parseLong(ticketCountValue));
            editableObject.setFirstTicketNumber(barcodeValue);
            stateCallBack.onCommitEdit(editableObject,notifiter);
        }
    }

    private void showErrors(List<String> errorMessages) {
        if(errorMessages!=null && !errorMessages.isEmpty()){

            errorMessage.setVisible(true);
            for(String error: errorMessages){
                errorMessage.add(new ListItem(error));
            }
        }else{
            errorMessage.clear();
            errorMessage.setVisible(false);
        }
    }

    @UiHandler("cancelButton")
    public void onCancel(ClickEvent event){
        if(this.editableObject!=null){
            stateCallBack.onCancelEdit(editableObject);
        }
    }


    interface TicketEditorUiBinder extends UiBinder<Panel, TicketEditor> {
    }

    private static TicketEditorUiBinder ourUiBinder = GWT.create(TicketEditorUiBinder.class);

}