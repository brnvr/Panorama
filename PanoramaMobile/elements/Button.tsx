import React from 'react';
import { Text, StyleSheet, Pressable } from 'react-native';
import { GestureDetector } from 'react-native-gesture-handler';
import Animated from 'react-native-reanimated';
import colors from '../def/colors';

type ButtonProps = {
    title: string;
    onPress?: () => void;
    style?: object;
};

export default function Button(props:ButtonProps) {
    const { onPress, style, title = 'Save' } = props;

    return (
        <Pressable style={[styles.button, style]} onPress={onPress}>
            <Text style={styles.text}>{title}</Text>
        </Pressable>
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
        borderColor: colors.accentYellow

    },
    text: {
        fontSize: 16,
        lineHeight: 21,
        fontWeight: 'bold',
        letterSpacing: 0.25,
        color: colors.accentYellow,
    },

    hoveredButton: {
        backgroundColor: 'lightblue',
    },
});

