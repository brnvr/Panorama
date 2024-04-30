import React from 'react';

import {
    Dimensions,
    SafeAreaView,
    ScrollView,
    StatusBar,
    Text,
    View
} from 'react-native';

import Button from './elements/Button'
import colors from './def/colors'
import MyTextInput from './elements/MyTextInput';

const { height: screenHeight } = Dimensions.get('window');

function App(): React.JSX.Element {
    const handleButtonPress = () => {
        fetch('http://10.0.3.2:80/panoramaapi/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                username: 'lol',
                password: 'lololololo'
            })
        })
        .then(r => {
            console.log(r.status)
        })
        .catch(r => {
            console.log(r.json())
        })
    };

    return (
        <SafeAreaView style={{ minHeight: screenHeight, backgroundColor: colors.background }}>
            <StatusBar backgroundColor='#091724' />

            <ScrollView
                contentInsetAdjustmentBehavior="automatic"
                style={{ backgroundColor: colors.background }}>

                <View style={{alignItems: 'center', padding: 15}}>
                    <MyTextInput placeholder="Username or e-mail" />
                    <MyTextInput placeholder="Password" />
                    <Button title="Sign in" onPress={handleButtonPress}  />
                    <Text style={{color: 'gray'}}>Reset password</Text>
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}

export default App;