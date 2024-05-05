import * as yup from 'yup'
import { Text, View } from "react-native"
import TextField from "../elements/TextField"
import { useForm } from "react-hook-form"
import { yupResolver } from "@hookform/resolvers/yup"
import { useEffect, useState } from "react"
import Button from '../elements/Button'
import axios, { AxiosError } from 'axios'
import Snackbar from 'react-native-snackbar'

function Login() {
    interface Login {
        username: string;
        password: string;
    }

    const fieldsValidationSchema = yup.object().shape({
        username: yup
            .string()
            .required("Username is required")
            .min(3, "Username must contain at least 3 digits")
            .max(30, "Username can't exceed 30 characters"),
        password: yup
            .string()
            .required("Password is required")
            .min(8, "Password must contain at least 8 digits")
            .max(72, "Password can't exceed 72 characters")
    })

    const { register, setValue, handleSubmit, formState: { errors }, setError, clearErrors } = useForm<Login>({ resolver: yupResolver(fieldsValidationSchema) })
    const [isButtonLoading, buttonSetLoading] = useState(false)

    function onSubmit(data: Login) {
        buttonSetLoading(true)
        axios.post('http://10.0.3.2:80/panoramaapi/auth/login', data)
            .then(r => {
                console.log(r)
            })
            .catch((error: AxiosError) => {
                buttonSetLoading(false)
                console.log(error)

                if (!error.response) {
                    return;
                }

                let msg;

                if (error.response.status == 401) {
                    msg = "Incorrect username/e-mail or password"
                }
                else {
                    msg = error.message;
                }

                Snackbar.show({
                    text: msg,
                    backgroundColor: "#c04856",
                });
            })
    };

    useEffect(() => {
        register('username')
        register('password')
    }, [register])

    function setFieldValue(name: keyof Login, value: string) {
        setValue(name, value)
        clearErrors(name)
    }

    return (
        <View style={{ alignItems: 'center', padding: 15 }}>
            <TextField placeholder="Username or e-mail" error={errors?.username} onChangeText={text => setFieldValue('username', text)} />
            <TextField placeholder="Password" error={errors?.password} onChangeText={text => setFieldValue("password", text)} />
            <Button title="Sign in" loading={isButtonLoading} onPress={handleSubmit(onSubmit)} />
            <Text style={{ color: '#BBBBBB', textDecorationLine: 'underline' }}>Reset password</Text>
        </View>
    )
}

export default Login