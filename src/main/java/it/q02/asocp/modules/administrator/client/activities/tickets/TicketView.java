package it.q02.asocp.modules.administrator.client.activities.tickets;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.user.cellview.client.TextColumn;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.base.client.data.TicketRoll;
import org.gwtbootstrap3.client.ui.Alert;
import org.gwtbootstrap3.client.ui.DataGrid;

/**
 * User: aleksander at  22.03.14, 19:09
 */
public class TicketView implements IsWidget {
    private Widget rootPanel;

    public TicketView(TicketActivity ticketActivity) {
           this();
    }

    @Override
    public Widget asWidget() {
        return rootPanel;
    }

    @UiField
    protected DataGrid<TicketRoll> dataContainer;

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
    }

    public static class TicketViewPresenter{

    }


    interface TicketViewUiBinder extends UiBinder<Widget, TicketView> {
    }

    private static TicketViewUiBinder ourUiBinder = GWT.create(TicketViewUiBinder.class);

}