import React from 'react';
import { TextInput, TextInputProps, StyleSheet, Text, View } from 'react-native';
import Colors from '../style/colors';
import { FieldError } from 'react-hook-form';

interface TextFieldProps extends TextInputProps {
    error?:FieldError
}

export default function TextField(props: TextFieldProps) {
    return (
        <View style={styles.view} >
            <TextInput {...props} style={[styles.input, !!props.error && styles.borderError]} placeholderTextColor={Colors.placeholder} />
            {!!props.error && !!props.error.message && < Text style={styles.errorMessage}>{props.error.message}</Text>}
        </View>
    );
}

const styles = StyleSheet.create({
    input: {
        height: 40,
        paddingHorizontal: 16,
        borderRadius: 3,
        marginBottom: 3,
        color: Colors.text,
        width: '100%',
        backgroundColor: Colors.field
    },

    view: {
        width: '100%',
        marginVertical: 10
    },

    errorMessage: {
        color: Colors.danger
    },

    borderError: {
        borderWidth: 1,
        borderColor: Colors.danger
    }
});

