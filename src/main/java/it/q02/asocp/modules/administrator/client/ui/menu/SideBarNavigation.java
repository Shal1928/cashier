package it.q02.asocp.modules.administrator.client.ui.menu;

import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.event.dom.client.ClickHandler;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.administrator.client.ui.menu.item.MenuItem;
import org.gwtbootstrap3.client.ui.ListItem;
import org.gwtbootstrap3.client.ui.Nav;

import java.util.HashMap;

/**
 * User: aleksander at  21.03.14, 9:12
 */
public class SideBarNavigation implements IsWidget, ClickHandler {
    private final Nav rootWidget;

    private HashMap<MenuItem,ListItem> itemHashMap;
    private HashMap<String,MenuItem>   menuKeys;

    private ListItem activeItem;

    @Override
    public Widget asWidget() {
        return rootWidget;
    }


    interface SideBarNavigationUiBinder extends UiBinder<Nav, SideBarNavigation> {
    }

    private static SideBarNavigationUiBinder ourUiBinder = GWT.create(SideBarNavigationUiBinder.class);

    @UiField
    protected Nav leftBar;

    public SideBarNavigation() {
        itemHashMap = new HashMap<>();
        menuKeys = new HashMap<>();
        rootWidget = ourUiBinder.createAndBindUi(this);
    }

    public void addItem(MenuItem item){
        if(!itemHashMap.containsKey(item)){
            ListItem child = new ListItem(item.getName());
            child.getElement().setAttribute("_menu_id",item.getId());
            leftBar.add(child);
            child.addClickHandler(this);
            itemHashMap.put(item,child);
            menuKeys.put(item.getId(),item);
        }
    }


    public void setActiveMenu(String id){
        if(menuKeys.containsKey(id)){
            setActiveMenuElement(menuKeys.get(id));
        }else{
            throw new ArrayIndexOutOfBoundsException("can`t find menu with id:"+id);
        }
    }

    public void setActiveMenu(MenuItem item){
       if(itemHashMap.containsKey(item)){
           setActiveMenuElement(item);
       }else{
           throw new ArrayIndexOutOfBoundsException("can`t find menu with id:"+item.getId());
       }
    }

    private void setActiveMenuElement(MenuItem item) {
        if(activeItem!=null){
            activeItem.setActive(false);
        }
        ListItem itemWidget = itemHashMap.get(item);
        itemWidget.setActive(true);
        activeItem = itemWidget;

    }

    @Override
    public void onClick(ClickEvent event) {
        GWT.log(event.getRelativeElement().toString());
        String menu_id = event.getRelativeElement().getParentElement().getAttribute("_menu_id");
        GWT.log("menu id:"+menu_id);
        menuKeys.get(menu_id).getCommand().execute();

    }


}