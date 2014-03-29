package it.q02.asocp.modules.administrator.client.activities.tickets;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
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
import it.q02.asocp.modules.administrator.client.rpc.TicketRollService;
import it.q02.asocp.modules.administrator.client.rpc.TicketRollServiceAsync;
import it.q02.asocp.modules.base.client.data.TicketRoll;
import it.q02.asocp.modules.base.client.ui.EditorStateCallback;
import it.q02.asocp.modules.base.client.ui.EditorStateCallbackProxy;
import org.gwtbootstrap3.client.ui.Alert;
import org.gwtbootstrap3.client.ui.DataGrid;

import java.util.Arrays;
import java.util.List;

/**
 * User: aleksander at  22.03.14, 19:09
 */
public class TicketView implements IsWidget {
    private Widget rootPanel;
    private TicketRollDataProvider rollProvider = new TicketRollDataProvider();
    private EditorStateCallbackProxy<TicketRoll> proxyListener;
    public TicketView(TicketActivity ticketActivity) {
           this();
    }

    @Override
    public Widget asWidget() {
        return rootPanel;
    }

    @UiField
    protected DataGrid<TicketRoll> dataContainer;
    @UiField
    protected TicketEditor ticketEditor;

    public TicketView() {
        rootPanel = ourUiBinder.createAndBindUi(this);
        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return Long.toString(ticketRoll.getId());
            }
        },"№");
        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            private  DateTimeFormat dtf = DateTimeFormat.getFormat("dd.MM.yyyy");
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return ticketRoll.getCreateDate()!=null ? dtf.format(ticketRoll.getCreateDate()) : "";
            }
        },"Создан");
        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return ticketRoll.getColor();
            }
        },"Цвет");
        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return ticketRoll.getFirstTicketSeries();
            }
        },"Серия");

        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return ticketRoll.getFirstTicketNumber();
            }
        },"Номер");

        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return  ticketRoll.getActivationInfo() == null ? "Нет" : "Да";

            }
        },"Активирован");
        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return Long.toString(ticketRoll.getTicketCount());
            }
        },"Остаток");
        dataContainer.setEmptyTableWidget(new Alert("Нет данных."));
        final SingleSelectionModel<TicketRoll> selectionModel = new SingleSelectionModel<TicketRoll>();
        dataContainer.setSelectionModel(selectionModel);
        selectionModel.addSelectionChangeHandler(new SelectionChangeEvent.Handler() {
            @Override
            public void onSelectionChange(SelectionChangeEvent selectionChangeEvent) {
                if(ticketEditor.getValue()!=null){
                   selectionModel.setSelected(ticketEditor.getValue(),true);
                }else{
                    ticketEditor.setValue(selectionModel.getSelectedObject(),proxyListener.callWith(new NewObjectCallbacks()));
                }
            }
        });
        rollProvider.addDataDisplay(dataContainer);
        rollProvider.reloadData();
    }

    public void setListener(EditorStateCallback<TicketRoll> listener){
        proxyListener=new EditorStateCallbackProxy<TicketRoll>(listener);
    }

    public void createNew() {
        TicketRoll ticketRoll = new TicketRoll();
        ticketRoll.setId(0);
        ticketEditor.setValue(ticketRoll, proxyListener.callWith(new NewObjectCallbacks()));
    }

    public boolean hasSelectedItems() {
        return false;  //To change body of created methods use File | Settings | File Templates.
    }

    interface TicketViewUiBinder extends UiBinder<Widget, TicketView> {
    }

    private static TicketViewUiBinder ourUiBinder = GWT.create(TicketViewUiBinder.class);

    private static class TicketRollDataProvider extends AsyncDataProvider<TicketRoll> implements AsyncCallback<List<TicketRoll>> {
        private TicketRollServiceAsync serviceAsync = TicketRollService.App.getInstance();

        public void doUpdate(){
            reloadData();
        }

        @Override
        protected void onRangeChanged(HasData<TicketRoll> ticketRollHasData) {
            reloadData();
        }

        private void reloadData() {
            serviceAsync.findTicketRolls(this);
        }


        @Override
        public void onFailure(Throwable throwable) {
            Window.alert("error, object");
        }

        @Override
        public void onSuccess(List<TicketRoll> ticketRollList) {
            updateRowData(0,ticketRollList);
        }
    }

    private class NewObjectCallbacks implements EditorStateCallback<TicketRoll> , AsyncCallback<TicketRoll>{
        private MessageNotifer notifiter;

        @Override
        public void onBeginEdit(TicketRoll editedObject) {

        }

        @Override
        public void onCommitEdit(TicketRoll editedObject, MessageNotifer notifiter) {
            this.notifiter = notifiter;
            TicketRollService.App.getInstance().saveAndUpdate(editedObject,this);
        }

        @Override
        public void onCancelEdit(TicketRoll editedObject) {
            ticketEditor.setValue(null,null);
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
        public void onSuccess(TicketRoll ticketRoll) {
            ticketEditor.setValue(null,null);
            rollProvider.reloadData();
        }
    }
}