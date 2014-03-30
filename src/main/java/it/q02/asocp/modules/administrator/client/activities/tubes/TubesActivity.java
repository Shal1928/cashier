package it.q02.asocp.modules.administrator.client.activities.tubes;

import com.google.gwt.activity.shared.Activity;
import com.google.gwt.event.shared.EventBus;
import com.google.gwt.user.client.Window;
import com.google.gwt.user.client.ui.AcceptsOneWidget;
import com.google.gwt.user.client.ui.Label;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.AddMenu;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.ByIdFilter;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.ChangeMenuState;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.ClearMenu;
import it.q02.asocp.modules.administrator.client.ui.menu.item.MenuWithIcon;
import it.q02.asocp.modules.base.client.data.Attraction;
import it.q02.asocp.modules.base.client.data.TicketRoll;
import it.q02.asocp.modules.base.client.ui.EditorStateCallback;
import org.gwtbootstrap3.client.ui.constants.IconType;

/**
 * User: aleksander at  22.03.14, 18:35
 */
public class TubesActivity implements Activity ,EditorStateCallback<Attraction>{

    private EventBus eventBus;
    private TubesView view;

    @Override
    public String mayStop() {
        return null;
    }

    @Override
    public void onCancel() {

    }

    @Override
    public void onStop() {
        eventBus.fireEvent(new ClearMenu());
    }

    @Override
    public void start(AcceptsOneWidget acceptsOneWidget, EventBus eventBus) {
        this.eventBus = eventBus;
        if(this.view == null){
            this.view = new TubesView();
            this.view.setListener(this);
        }
        acceptsOneWidget.setWidget(this.view);
        eventBus.fireEvent(new AddMenu(new MenuWithIcon("Создать","create", IconType.PLUS){
            @Override
            public void execute() {
                view.createNew();
            }
        }));
        eventBus.fireEvent(new AddMenu(new MenuWithIcon("Править","edit",IconType.EDIT){
            @Override
            public void execute() {

            }
        }));
        eventBus.fireEvent(new AddMenu(new MenuWithIcon("Удалить","delete",IconType.ERASER){
            @Override
            public void execute() {
                Window.alert("delete");
            }
        }));
        eventBus.fireEvent(new ChangeMenuState(ChangeMenuState.State.DISABLED, new ByIdFilter("edit","delete")));
    }

    @Override
    public void onBeginEdit(Attraction editedObject) {
        eventBus.fireEvent(new ChangeMenuState(ChangeMenuState.State.DISABLED, new ByIdFilter("create","edit","delete")));
    }

    @Override
    public void onCommitEdit(Attraction editedObject, MessageNotifer notifiter) {
        if(view.hasSelectedItems()){
            eventBus.fireEvent(new ChangeMenuState(ChangeMenuState.State.ENABLED, new ByIdFilter("create","edit","delete")));
        }else{
            eventBus.fireEvent(new ChangeMenuState(ChangeMenuState.State.ENABLED, new ByIdFilter("create")));
        }
    }

    @Override
    public void onCancelEdit(Attraction editedObject) {
        if(view.hasSelectedItems()){
            eventBus.fireEvent(new ChangeMenuState(ChangeMenuState.State.ENABLED, new ByIdFilter("create","edit","delete")));
        }else{
            eventBus.fireEvent(new ChangeMenuState(ChangeMenuState.State.ENABLED, new ByIdFilter("create")));
        }
    }

}
