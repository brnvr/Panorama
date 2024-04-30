import React from 'react';
import { Text, TextInput, StyleSheet, Pressable } from 'react-native';
import colors from '../def/colors';

type TextInputProps = {
    placeholder: string
};

export default function MyTextInput(props: TextInputProps) {
    const { placeholder } = props;

    return (
        <TextInput
            style={styles.input}
            placeholder={placeholder}
            placeholderTextColor="gray"
        />
    );
}

const styles = StyleSheet.create({
    input: {
        height: 40,
        paddingVertical: 12,
        paddingHorizontal: 32,
        margin: 10,
        borderRadius: 3,
        borderColor: 'gray',
        color: 'white',
        width: '100%',
        textAlign: 'center',
        backgroundColor: '#233145'
        // Add any additional styles you want to apply to all text inputs
    },
});

