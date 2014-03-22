package it.q02.asocp.modules.administrator.client.ui.menu;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.event.dom.client.ClickHandler;
import com.google.gwt.event.shared.HandlerRegistration;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.user.client.ui.HasEnabled;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import com.google.web.bindery.event.shared.EventBus;
import it.q02.asocp.modules.administrator.client.ui.menu.bus.*;
import it.q02.asocp.modules.administrator.client.ui.menu.item.HasCustomRenderer;
import it.q02.asocp.modules.administrator.client.ui.menu.item.HasIcon;
import it.q02.asocp.modules.administrator.client.ui.menu.item.IsUpdatable;
import it.q02.asocp.modules.administrator.client.ui.menu.item.MenuItem;
import org.gwtbootstrap3.client.ui.ListItem;
import org.gwtbootstrap3.client.ui.Navbar;
import org.gwtbootstrap3.client.ui.NavbarHeader;
import org.gwtbootstrap3.client.ui.NavbarNav;
import org.gwtbootstrap3.client.ui.constants.IconType;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

/**
 * User: aleksander at  21.03.14, 9:07
 */
public class MainMenu implements IsWidget,ClearMenuHandler,AddMenuHandler,ChangeMenuStateHandler,UpdateMenuItemsHandler,ClickHandler {

    private final Navbar menu;

    @UiField
    protected NavbarNav contextMenu;

    private HashMap<MenuItem,Object> renderedElements = new HashMap<>();
    private HashMap<String,MenuItem>   menuKeys = new HashMap<>();
    private List<HandlerRegistration> registrationList= new LinkedList<>();
    public MainMenu() {
        menu = ourUiBinder.createAndBindUi(this);

    }


    @Override
    public Widget asWidget() {
        return menu;  //To change body of implemented methods use File | Settings | File Templates.
    }

    @Override
    public void onAddMenu(AddMenu event) {
        MenuItem item = event.getItem();
        if(item instanceof HasCustomRenderer){
            contextMenu.add((HasCustomRenderer)item);
            menuKeys.put(item.getId(),item);
            renderedElements.put(item,item);
        }else {
            ListItem renderedItem = new ListItem(item.getName());
            if(item instanceof HasIcon){
                renderedItem.setIcon(((HasIcon) item).getIcon());
            }
            renderedItem.getElement().setAttribute("_menu_id",item.getId());
            registrationList.add(renderedItem.addClickHandler(this));
            menuKeys.put(item.getId(),item);
            renderedElements.put(item,renderedItem);
            contextMenu.add(renderedItem);
        }
    }

    @Override
    public void onClearMenu(ClearMenu event) {
        for(HandlerRegistration r : registrationList){
            r.removeHandler();
        }
        contextMenu.clear();
        registrationList.clear();
    }

    @Override
    public void onChangeMenuState(ChangeMenuState event) {
        for(Map.Entry<MenuItem, Object> item:renderedElements.entrySet()){
            if(event.getFilter().applyStateTo(item.getKey())){
                Object value = item.getValue();
                if(value instanceof HasEnabled){
                    ((HasEnabled) value).setEnabled(ChangeMenuState.State.ENABLED.equals(event.getNewState()));
                } else if( value instanceof org.gwtbootstrap3.client.ui.HasEnabled){
                    ((org.gwtbootstrap3.client.ui.HasEnabled) value).setEnabled(ChangeMenuState.State.ENABLED.equals(event.getNewState()));
                }

            }
        }
    }

    @Override
    public void onUpdateMenuItems(UpdateMenuItems event) {
        for(Map.Entry<MenuItem, Object> item:renderedElements.entrySet()){
            MenuItem key = item.getKey();
            if(event.getFilter().applyStateTo(key)){
               Object value = item.getValue();
                if(value instanceof ListItem){
                    ((ListItem) value).setText(key.getName());
                    if(key instanceof HasIcon){
                        ((ListItem) value).setIcon(((HasIcon) key).getIcon());
                    }
                }else if( value  instanceof IsUpdatable){
                    ((IsUpdatable) value).updateItem();
                }
            }
        }
    }

    @Override
    public void onClick(ClickEvent event) {
        GWT.log(event.getRelativeElement().toString());
        String menu_id = event.getRelativeElement().getParentElement().getAttribute("_menu_id");
        GWT.log("menu id:"+menu_id);
        menuKeys.get(menu_id).getCommand().execute();
    }

    interface MainMenuUiBinder extends UiBinder<Navbar, MainMenu> {
    }

    private static MainMenuUiBinder ourUiBinder = GWT.create(MainMenuUiBinder.class);


}