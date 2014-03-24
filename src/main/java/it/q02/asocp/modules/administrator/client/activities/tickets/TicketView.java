package it.q02.asocp.modules.administrator.client.activities.tickets;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.user.cellview.client.TextColumn;
import com.google.gwt.user.client.Window;
import com.google.gwt.user.client.rpc.AsyncCallback;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import com.google.gwt.view.client.AsyncDataProvider;
import com.google.gwt.view.client.HasData;
import it.q02.asocp.modules.administrator.client.rpc.TicketRollService;
import it.q02.asocp.modules.administrator.client.rpc.TicketRollServiceAsync;
import it.q02.asocp.modules.base.client.data.TicketRoll;
import it.q02.asocp.modules.base.client.ui.EditorStateCallback;
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
                return "1";
            }
        },"№");
        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return "";
            }
        },"Создан");
        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return "1";
            }
        },"Цвет");
        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return "1";
            }
        },"Первый билет");
        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return "1";
            }
        },"Активирован");
        dataContainer.addColumn(new TextColumn<TicketRoll>() {
            @Override
            public String getValue(TicketRoll ticketRoll) {
                return "1";
            }
        },"Остаток");
        dataContainer.setEmptyTableWidget(new Alert("Нет данных."));
        rollProvider.addDataDisplay(dataContainer);
        rollProvider.reloadData();
    }

    public void createNew() {
      ticketEditor.setValue(new TicketRoll(), new NewObjectCallbacks());
    }
                                                         //To change body of implemented methods use File | Settings | File Templates.
    public static class TicketViewPresenter{

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
            Window.alert("Fuck, error");
        }

        @Override
        public void onSuccess(List<TicketRoll> ticketRollList) {
            updateRowData(0,ticketRollList);
        }
    }

    private static class NewObjectCallbacks implements EditorStateCallback<TicketRoll> {
        @Override
        public void onBeginEdit(TicketRoll editedObject) {
            //todo: Заблокировать таблицу
        }

        @Override
        public void onCommitEdit(TicketRoll editedObject, MessageNotifer notifiter) {
          //todo: Сходить на сервер
        }

        @Override
        public void onCancelEdit(TicketRoll editedObject) {
            //todo: нечего не делать
        }
    }
}