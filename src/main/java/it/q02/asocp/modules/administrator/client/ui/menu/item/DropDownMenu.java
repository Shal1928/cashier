package it.q02.asocp.modules.administrator.client.ui.menu.item;

import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.event.dom.client.ClickHandler;
import com.google.gwt.user.client.ui.Widget;
import org.gwtbootstrap3.client.ui.Anchor;
import org.gwtbootstrap3.client.ui.AnchorButton;
import org.gwtbootstrap3.client.ui.ListDropDown;
import org.gwtbootstrap3.client.ui.ListItem;
import org.gwtbootstrap3.client.ui.constants.ButtonType;
import org.gwtbootstrap3.client.ui.constants.IconType;
import org.gwtbootstrap3.client.ui.constants.Toggle;

/**
 * User: aleksander at  31.03.14, 17:45
 */
public class DropDownMenu extends AbstractMenu implements HasCustomRenderer  {

    private ListDropDown listDropDown;



    public DropDownMenu(String name, String id, final MenuItem... menuItems) {
        super(name,id);
        AnchorButton child = new AnchorButton(ButtonType.DEFAULT);
        child.setToggle(Toggle.DROPDOWN);
        child.setText(name);
        listDropDown = new ListDropDown();
        listDropDown.add(child);

        org.gwtbootstrap3.client.ui.DropDownMenu menu = new org.gwtbootstrap3.client.ui.DropDownMenu();
        for(final MenuItem item:menuItems){

            ListItem renderedItem = new ListItem(item.getName());
            if(item instanceof HasIcon){
                renderedItem.setIcon(((HasIcon) item).getIcon());
            }
            renderedItem.addClickHandler(new ClickHandler() {
                @Override
                public void onClick(ClickEvent event) {
                    item.getCommand().execute();
                }
            });
            menu.add(renderedItem);
        }
        listDropDown.add(menu);
    }

    @Override
    public void execute() {

    }

    @Override
    public Widget asWidget() {
        return listDropDown;
    }

}
