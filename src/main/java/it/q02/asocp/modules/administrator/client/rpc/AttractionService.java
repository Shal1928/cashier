package it.q02.asocp.modules.administrator.client.rpc;

import com.google.gwt.user.client.rpc.RemoteService;
import com.google.gwt.user.client.rpc.RemoteServiceRelativePath;
import com.google.gwt.core.client.GWT;
import it.q02.asocp.modules.base.client.data.Attraction;

import java.util.List;

/**
 * User: aleksander at  29.03.14, 20:43
 */
@RemoteServiceRelativePath("AttractionService")
public interface AttractionService extends RemoteService {

    public Attraction saveAndUpdate(Attraction attraction);

    public List<Attraction> find();

    /**
     * Utility/Convenience class.
     * Use AttractionService.App.getInstance() to access static instance of AttractionServiceAsync
     */
    public static class App {
        private static final AttractionServiceAsync ourInstance = (AttractionServiceAsync) GWT.create(AttractionService.class);

        public static AttractionServiceAsync getInstance() {
            return ourInstance;
        }
    }
}
