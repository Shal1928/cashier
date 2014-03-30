package it.q02.asocp.modules.administrator.client.activities.tubes;

import com.google.gwt.core.client.GWT;
import com.google.gwt.i18n.client.DateTimeFormat;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.user.cellview.client.TextColumn;
import com.google.gwt.user.client.Window;
import com.google.gwt.user.client.rpc.AsyncCallback;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import com.google.gwt.view.client.AsyncDataProvider;
import com.google.gwt.view.client.HasData;
import com.google.gwt.view.client.SelectionChangeEvent;
import com.google.gwt.view.client.SingleSelectionModel;
import it.q02.asocp.modules.administrator.client.rpc.AttractionService;
import it.q02.asocp.modules.administrator.client.rpc.AttractionServiceAsync;
import it.q02.asocp.modules.administrator.client.rpc.TicketRollService;
import it.q02.asocp.modules.base.client.data.Attraction;
import it.q02.asocp.modules.base.client.data.TicketRoll;
import it.q02.asocp.modules.base.client.ui.EditorStateCallback;
import it.q02.asocp.modules.base.client.ui.EditorStateCallbackProxy;
import it.q02.asocp.modules.base.client.ui.editor.BaseEditor;
import org.gwtbootstrap3.client.ui.Alert;
import org.gwtbootstrap3.client.ui.DataGrid;

import java.util.List;

/**
 * User: aleksander at  22.03.14, 19:09
 */
public class TubesView implements IsWidget {

    private Widget rootPanel;
    private AttractionDataProvider rollProvider = new AttractionDataProvider();
    private EditorStateCallbackProxy<Attraction> proxyListener;

    @Override
    public Widget asWidget() {
        return rootPanel;
    }

    @UiField
    protected DataGrid<Attraction> dataContainer;

    @UiField
    protected BaseEditor<Attraction> attractionEditor;

    public TubesView() {
        rootPanel = ourUiBinder.createAndBindUi(this);

        dataContainer.setEmptyTableWidget(new Alert("Нет данных."));
        final SingleSelectionModel<Attraction> selectionModel = new SingleSelectionModel<Attraction>();
        dataContainer.setSelectionModel(selectionModel);

        selectionModel.addSelectionChangeHandler(new SelectionChangeEvent.Handler() {
            @Override
            public void onSelectionChange(SelectionChangeEvent selectionChangeEvent) {
                if(attractionEditor.getValue()!=null){
                   selectionModel.setSelected(attractionEditor.getValue(),true);
                }else{
                    attractionEditor.setValue(selectionModel.getSelectedObject(), proxyListener.callWith(new NewObjectCallbacks()));
                }
            }
        });
        dataContainer.addColumn(new TextColumn<Attraction>() {
            @Override
            public String getValue(Attraction attraction) {
                return attraction.getCode();
            }
        },"№");
        dataContainer.addColumn(new TextColumn<Attraction>() {
            @Override
            public String getValue(Attraction attraction) {
                return attraction.getName();
            }
        },"Название");
        dataContainer.addColumn(new TextColumn<Attraction>() {
            @Override
            public String getValue(Attraction attraction) {
                return Float.toString(attraction.getPrice()/100.00f);
            }
        },"Цена");
        dataContainer.addColumn(new TextColumn<Attraction>() {
            @Override
            public String getValue(Attraction attraction) {
                return Float.toString(attraction.getMinPrice()/100.00f);
            }
        },"Мин.Цена");

        rollProvider.addDataDisplay(dataContainer);
        rollProvider.reloadData();
    }

    public void setListener(EditorStateCallback<Attraction> listener){
        proxyListener=new EditorStateCallbackProxy<Attraction>(listener);
    }

    public void createNew() {
        Attraction attraction = new Attraction();
        attraction.setVersionSeries(null);
        attraction.setId(0);
        attractionEditor.setValue(attraction, proxyListener.callWith(new NewObjectCallbacks()));
    }

    public boolean hasSelectedItems() {
        return false;  //To change body of created methods use File | Settings | File Templates.
    }

    interface TubiesViewUiBinder extends UiBinder<Widget, TubesView> {
    }

    private static TubiesViewUiBinder ourUiBinder = GWT.create(TubiesViewUiBinder.class);

    private static class AttractionDataProvider extends AsyncDataProvider<Attraction> implements AsyncCallback<List<Attraction>> {
        private AttractionServiceAsync serviceAsync = AttractionService.App.getInstance();

        public void doUpdate(){
            reloadData();
        }

        @Override
        protected void onRangeChanged(HasData<Attraction> ticketRollHasData) {
            reloadData();
        }

        private void reloadData() {
            serviceAsync.find(this);
        }


        @Override
        public void onFailure(Throwable throwable) {
            Window.alert("error, object");
        }

        @Override
        public void onSuccess(List<Attraction> ticketRollList) {
            updateRowData(0,ticketRollList);
        }
    }

    private class NewObjectCallbacks implements EditorStateCallback<Attraction> , AsyncCallback<Attraction>{
        private MessageNotifer notifiter;

        @Override
        public void onBeginEdit(Attraction editedObject) {

        }

        @Override
        public void onCommitEdit(Attraction editedObject, MessageNotifer notifiter) {
            this.notifiter = notifiter;
            AttractionService.App.getInstance().saveAndUpdate(editedObject,this);
        }

        @Override
        public void onCancelEdit(Attraction editedObject) {
            attractionEditor.setValue(null, null);
        }

        @Override
        public void onFailure(Throwable throwable) {
            if(notifiter!=null){
                notifiter.addMessage(throwable.getMessage());
                if(throwable instanceof TicketRollService.TicketRollServiceException){
                    notifiter.addMessage(((TicketRollService.TicketRollServiceException) throwable).getError().getErrorDescribe());
                }
                notifiter.showMessages();
            }
        }

        @Override
        public void onSuccess(Attraction ticketRoll) {
            attractionEditor.setValue(null, null);
            rollProvider.reloadData();
        }
    }
}