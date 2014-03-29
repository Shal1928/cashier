package it.q02.asocp.modules.administrator.client.activities.tickets;

import com.google.gwt.core.client.GWT;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.uibinder.client.UiHandler;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.base.client.data.TicketRoll;
import it.q02.asocp.modules.base.client.ui.EditorStateCallback;
import org.gwtbootstrap3.client.ui.*;

import java.util.LinkedList;
import java.util.List;

/**
 * User: aleksander at  24.03.14, 17:27
 */
public class TicketEditor implements IsWidget   {
    private final Panel rootPanel;
    private TicketRoll editableObject;

    @UiField
    protected TextBox ticketSeries;
    @UiField
    protected TextBox ticketColor;
    @UiField
    protected PanelFooter buttonGroup;
    @UiField
    protected TextBox ticketCount;
    @UiField
    protected Alert errorMessage;
    @UiField
    protected TextBox ticketNumber;

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


    public TicketEditor() {
        rootPanel = ourUiBinder.createAndBindUi(this);
        rootPanel.setVisible(false);
    }

    @UiHandler("saveButton")
    public void onCommit(ClickEvent event){
        List<String> errorMessages = new LinkedList<String>();

        String seriesValue = ticketSeries.getText().trim();
        if(seriesValue.isEmpty()){
            errorMessages.add("Серия не может быть пустой!");
        }

        String numberValue = ticketNumber.getText().trim();
        if(seriesValue.isEmpty()){
            errorMessages.add("Номер не может быть пустой!");
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
            editableObject.setFirstTicketNumber(numberValue);
            editableObject.setFirstTicketSeries(seriesValue);
            stateCallBack.onCommitEdit(editableObject,notifiter);
        }
    }

    @UiHandler("cancelButton")
    public void onCancel(ClickEvent event){
        if(this.editableObject!=null){
            stateCallBack.onCancelEdit(editableObject);
        }
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



    public TicketRoll getValue() {
        return editableObject;
    }

    @Override
    public Widget asWidget() {
        return rootPanel;
    }

    private void changeState(boolean newState) {
        ticketSeries.setEnabled(newState);
        ticketNumber.setEnabled(newState);
        ticketColor.setEnabled(newState);
        ticketCount.setEnabled(newState);
        buttonGroup.setVisible(newState);
    }

    private void formClearWith(TicketRoll ticketRoll) {
        showErrors(null);
        if(ticketRoll != null){
            rootPanel.setVisible(true);
            ticketColor.setText(ticketRoll.getColor());
            ticketSeries.setText(ticketRoll.getFirstTicketSeries());
            ticketNumber.setText(ticketRoll.getFirstTicketNumber());
            ticketCount.setText(Long.toString(ticketRoll.getTicketCount()));
        }else{
            rootPanel.setVisible(false);
            ticketColor.setText("");
            ticketSeries.setText("");
            ticketSeries.setText("");
            ticketCount.setText("");
        }
        ticketSeries.setEnabled(false);
        ticketColor.setEnabled(false);
        buttonGroup.setVisible(false);
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

    interface TicketEditorUiBinder extends UiBinder<Panel, TicketEditor> {
    }

    private static TicketEditorUiBinder ourUiBinder = GWT.create(TicketEditorUiBinder.class);

}