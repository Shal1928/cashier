package it.q02.asocp.modules.administrator.client.ui.menu.item;

import com.google.gwt.activity.shared.ActivityManager;
import com.google.gwt.place.shared.Place;
import com.google.gwt.place.shared.PlaceController;

/**
 * User: aleksander at  22.03.14, 17:57
 */
public class ChangePlace extends AbstractMenu {

    private Place place;
    private final PlaceController placeController;


    public ChangePlace(String name, Place place,PlaceController placeController) {
        super(name,place.getClass().getName());
        this.place = place;
        this.placeController = placeController;
    }

    @Override
    public void execute() {
        placeController.goTo(place);
    }
}
