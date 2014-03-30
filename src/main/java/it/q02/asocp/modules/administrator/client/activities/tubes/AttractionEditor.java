package it.q02.asocp.modules.administrator.client.activities.tubes;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.uibinder.client.UiHandler;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Label;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.base.client.data.Attraction;
import it.q02.asocp.modules.base.client.ui.EditorStateCallback;
import it.q02.asocp.modules.base.client.ui.editor.BaseEditor;
import org.gwtbootstrap3.client.ui.Alert;
import org.gwtbootstrap3.client.ui.ListItem;
import org.gwtbootstrap3.client.ui.PanelFooter;
import org.gwtbootstrap3.client.ui.TextBox;

import javax.validation.Validation;
import javax.validation.Validator;
import java.util.LinkedList;
import java.util.List;

/**
 * User: aleksander at  29.03.14, 20:50
 */
public class AttractionEditor implements BaseEditor<Attraction> {

    private final Widget rootWidget;
    private EditorStateCallback<Attraction> editorCallBack;
    private Attraction editedObject;


    @UiField
    protected PanelFooter buttonGroup;
    @UiField
    protected TextBox attractionCode;
    @UiField
    protected TextBox attractionName;
    @UiField
    protected TextBox attractionPrintName;
    @UiField
    protected TextBox attractionPrice;
    @UiField
    protected TextBox attractionMinPrice;
    @UiField
    protected Alert errorMessage;

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

    public AttractionEditor() {
        rootWidget = ourUiBinder.createAndBindUi(this);
        rootWidget.setVisible(false);
    }




    @Override
    public void setValue(Attraction attraction, EditorStateCallback<Attraction> callBack) {
        this.editedObject =attraction;
        this.editorCallBack = callBack;
        updateFields(attraction);

    }

    private void updateFields(Attraction attraction) {
        if(attraction==null){
            rootWidget.setVisible(false);
            attractionCode.setText("");
            attractionName.setText("");
            attractionPrintName.setText("");
            attractionPrice.setText("");
            attractionMinPrice.setText("");
        }else{
            rootWidget.setVisible(true);
            attractionCode.setText(attraction.getCode());
            attractionName.setText(attraction.getName());
            attractionPrintName.setText(attraction.getPrintName());
            attractionPrice.setText(Float.toString(attraction.getPrice() / 100.00f));
            attractionMinPrice.setText(Float.toString(attraction.getMinPrice() / 100.00f));
            editorCallBack.onBeginEdit(attraction);
        }
    }

    @Override
    public Attraction getValue() {
        return editedObject;
    }



    @UiHandler("saveButton")
    public void onSave(ClickEvent event){
        editedObject.setCode(attractionCode.getText());
        editedObject.setName(attractionName.getText());
        editedObject.setPrintName(attractionPrintName.getText());
        editedObject.setPrice((long) (Float.parseFloat(attractionPrice.getText())*100));
        editedObject.setMinPrice((long) (Float.parseFloat(attractionPrice.getText())*100));
        editorCallBack.onCommitEdit(editedObject,notifiter);
    }

    @UiHandler("cancelButton")
    public void onCancel(ClickEvent event){
        if(this.editedObject!=null){
            editorCallBack.onCancelEdit(editedObject);
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

    @Override
    public Widget asWidget() {
        return rootWidget;
    };

    interface AttractionEditorUiBinder extends UiBinder<Widget, AttractionEditor> {
    }

    private static AttractionEditorUiBinder ourUiBinder = GWT.create(AttractionEditorUiBinder.class);


}