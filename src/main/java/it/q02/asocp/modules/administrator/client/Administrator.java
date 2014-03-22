package it.q02.asocp.modules.administrator.client;

import com.google.gwt.activity.shared.ActivityManager;
import com.google.gwt.core.client.EntryPoint;
import com.google.gwt.core.client.GWT;
import com.google.gwt.place.shared.Place;
import com.google.gwt.place.shared.PlaceController;
import com.google.gwt.place.shared.PlaceHistoryHandler;
import com.google.gwt.user.client.ui.*;
import com.google.web.bindery.event.shared.EventBus;
import it.q02.asocp.modules.administrator.client.activities.tickets.TicketPlace;
import it.q02.asocp.modules.administrator.client.activities.tubes.TubesPlace;
import it.q02.asocp.modules.administrator.client.ui.layout.MainLayout;
import it.q02.asocp.modules.administrator.client.ui.menu.MainMenu;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.AddMenu;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.ChangeMenuState;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.ClearMenu;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.UpdateMenuItems;
import it.q02.asocp.modules.administrator.client.ui.menu.item.*;

/**
 * User: aleksander at  16.03.14, 18:34
 */
public class Administrator implements EntryPoint {

    private Place defaultPlace  = Place.NOWHERE;


    public void onModuleLoad() {

        MainMenu topMenu = new MainMenu();
        RootPanel.get().add(topMenu);
        MainLayout mainLayout = new MainLayout();
        RootPanel.get().add(mainLayout);


        ClientFactory factory = GWT.create(ClientFactory.class);

        EventBus eventBus = factory.getEventBus();
        PlaceController controller = factory.getPlaceController();
        /**
         * todo: Сделать файл конфиграции
         * todo: Сделать привязку к ролям
         * todo:
         */
        mainLayout.getSideBar().addItem(new ChangePlace("Билеты",new TicketPlace(),controller));
        mainLayout.getSideBar().addItem(new ChangePlace("Атракционы",new TubesPlace(),controller));

        eventBus.addHandler(AddMenu.TYPE,topMenu);
        eventBus.addHandler(ChangeMenuState.TYPE,topMenu);
        eventBus.addHandler(ClearMenu.TYPE,topMenu);
        eventBus.addHandler(UpdateMenuItems.TYPE,topMenu);

        ActivityMapper activityMapper = new ActivityMapper(factory,mainLayout);
        ActivityManager manager = new ActivityManager(activityMapper,eventBus);
        manager.setDisplay(new ContainerWrapper(mainLayout));

        PlaceHistoryMapper historyMapper = GWT.create(PlaceHistoryMapper.class);
        PlaceHistoryHandler handler = new PlaceHistoryHandler(historyMapper);
        handler.register(controller,eventBus,defaultPlace);

        handler.handleCurrentHistory();
    }

    private static class ContainerWrapper implements AcceptsOneWidget {
        private final MainLayout mainLayout;
        private IsWidget lastWidget;

        public ContainerWrapper(MainLayout mainLayout) {
            this.mainLayout = mainLayout;
        }

        @Override
        public void setWidget(IsWidget isWidget) {
            if(this.lastWidget!=null){
                mainLayout.getContainer().remove(this.lastWidget);
            }
            this.lastWidget = isWidget;
            if(isWidget!=null){
                mainLayout.getContainer().add(isWidget);
            }
        }
    }
}
