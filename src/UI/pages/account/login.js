import { useState } from 'react';
import { useRouter } from 'next/router';
import { userService } from '../../services/user-service';

export default function LoginPage() {
    const router = useRouter();
    const [email, setEmail] = useState();
    const [request, setRequest] = useState({});

    const emailChanged = (e) => {
        setEmail(e.target.value);
    }

    const login = async (e) => {
        e.preventDefault();

        setRequest({ loading: true });
        const user = await userService.login(email).catch(e => {
            setRequest({ loading: false, error: e });
        });

        if (user) {
            setRequest({ loading: false });
            router.push('/');
        }
    }

    return (
        <div className="container px-6 mx-auto grid">
            <div className="my-6 px-4 py-3 mb-8 bg-white rounded-lg shadow-md">
                <div className="flex items-center justify-center p-6 sm:p-12 md:w-1/2">
                    <div className="w-full">
                        <h1 className="mb-4 text-xl font-semibold text-gray-700">
                            Login
                        </h1>
                        <form onSubmit={login}>
                            <label className="block text-sm">
                                <span className="text-gray-700">Email</span>
                                <input
                                    className="block w-full mt-1 text-sm focus:border-purple-400 focus:outline-none focus:shadow-outline-purple form-input"
                                    placeholder="Enter your email"
                                    onChange={emailChanged} />
                            </label>

                            <button
                                type="submit"
                                disabled={request.loading}
                                className="block w-full px-4 py-2 mt-4 text-sm font-medium leading-5 text-center text-white transition-colors duration-150 bg-purple-600 border border-transparent rounded-lg active:bg-purple-600 hover:bg-purple-700 focus:outline-none focus:shadow-outline-purple">
                                {request.loading ? 'Loading...' : 'Log in'}
                            </button>
                        </form>
                        {request.error && <span className="my-6 text-red-700">{request.error}</span>}
                    </div>
                </div>
            </div>
        </div>
    )
}