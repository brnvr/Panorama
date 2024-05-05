import {
    Dimensions,
    SafeAreaView,
    ScrollView,
    StatusBar
} from 'react-native';

import Spinner from 'react-native-loading-spinner-overlay'
import Colors from './style/colors';
import Login from './views/login';

function App(): React.JSX.Element {
    const { height: screenHeight } = Dimensions.get('window');

    return (
        <SafeAreaView style={{ minHeight: screenHeight, backgroundColor: Colors.background }}>
            <StatusBar backgroundColor={Colors.statusBar} />
            
            <ScrollView
                contentInsetAdjustmentBehavior="automatic"
                style={{ backgroundColor: Colors.background }}>

                <Login></Login>
            </ScrollView>
        </SafeAreaView>
    );
}

export default App