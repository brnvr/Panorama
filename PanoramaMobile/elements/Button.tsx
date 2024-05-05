import React from 'react';
import { Text, StyleSheet, TouchableOpacity, ActivityIndicator, TouchableOpacityProps, View } from 'react-native';
import Colors from '../style/colors';
import Spinner from 'react-native-spinkit'
interface ButtonProps extends TouchableOpacityProps {
    title: string,
    loading?: boolean
};

export default function Button(props: ButtonProps) {
    const disabled = (props.loading === undefined || props.loading === null) ? props.disabled : props.loading

    return (
        <TouchableOpacity {...props} disabled={disabled} style={[styles.button, props.style]}>
            <Text style={styles.text}>{props.title}</Text>
            {!!props.loading && (
                <View style={styles.spinnerContainer}>
                    <Spinner color={Colors.primary} type={'ChasingDots'} style={styles.spinner} />
                </View>
            )}
        </TouchableOpacity>
    );
}

const styles = StyleSheet.create({
    button: {
        alignItems: 'center',
        width: '100%',
        paddingVertical: 12,
        paddingHorizontal: 32,
        marginVertical: 10,
        borderRadius: 9,
        borderWidth: 2,
        borderColor: Colors.primary,
        flexDirection: 'row',
        justifyContent: 'center'
        
    },
    text: {
        fontSize: 16,
        lineHeight: 21,
        fontWeight: 'bold',
        letterSpacing: 0.25,
        color: Colors.primary,
    },
    spinnerContainer: {
        marginLeft: 10,
        justifyContent: 'center',
        alignContent: 'center'
    },
    spinner: {
        bottom: -16,
        position: 'absolute'
    }
});

