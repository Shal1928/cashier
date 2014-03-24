package it.q02.asocp.modules.administrator.client.ui.menu.bus;

import it.q02.asocp.modules.administrator.client.ui.menu.bus.ApplyToFilter;
import it.q02.asocp.modules.administrator.client.ui.menu.item.MenuItem;

import java.util.Arrays;
import java.util.List;

/**
* User: aleksander at  23.03.14, 10:25
*/
public class ByIdFilter implements ApplyToFilter {
    private List<String> ids;

    public ByIdFilter(String... arg1) {
        this.ids = Arrays.asList(arg1);
    }

    @Override
    public boolean applyStateTo(MenuItem item) {
        return ids.contains(item.getId());
    }
}
