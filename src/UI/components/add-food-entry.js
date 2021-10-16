import { useState, useEffect } from 'react';
import { foodEntryService } from '../services/food-entry-service';
import { userService } from '../services/user-service';
import { alertService } from '../services/alert-service';
import moment from 'moment';

export default function AddFoodEntry({ isOpen, close, isAdmin, selectedFoodEntry }) {
    const [foodEntry, setFoodEntry] = useState({ initial: true, user: { id: 'me' }, date: moment(new Date()).format('yyyy-MM-DD'), foodName: '', calorie: '', cheatMeal: false });
    const [error, setError] = useState({});
    const [users, setUsers] = useState([]);
    const [request, setRequest] = useState({});

    useEffect(() => {
        if (isOpen && isAdmin) {
            if (selectedFoodEntry) {
                setFoodEntry({ ...foodEntry, ...selectedFoodEntry, date: moment(selectedFoodEntry.date).format('yyyy-MM-DD'), cheatMeal: selectedFoodEntry.calorie === 0 });
            }
            userService.getUsers().then(users => {
                setUsers(users);
            });
        }

        return () => { setUsers([]); }
    }, [isOpen]);

    const valueChanged = (e) => {
        setFoodEntry({ ...foodEntry, initial: false, [e.target.name]: e.target.value });
        setError({ ...error, [e.target.name]: !e.target.value })
    }

    const checkValues = () => {
        const formError = {
            date: !foodEntry.date,
            foodName: !foodEntry.foodName,
            calorie: !foodEntry.cheatMeal && (!foodEntry.calorie || foodEntry.calorie < 1),
            userId: !foodEntry.user.id,
        }
        setError(formError);

        return formError.date || formError.foodName || formError.calorie || formError.userId;
    }

    const saveFoodEntry = async (e) => {
        e.preventDefault();

        if (!checkValues()) {
            return;
        }

        setRequest({ loading: true });

        if (foodEntry.id) {
            await updateFoodEntry();
            return;
        }

        const result = await addFoodEntry();

        if (!result)
            return;

        setRequest({ loading: false, success: true });
        setFoodEntry({ ...foodEntry, initial: true, foodName: '', calorie: '' });
    }

    const addFoodEntry = async () => {
        return await foodEntryService.addFoodEntry(foodEntry.user.id, getFoodEntry()).catch(e => {
            setRequest({ ...request, error: e });
            return null;
        });
    }

    const updateFoodEntry = async () => {
        await foodEntryService.updateFoodEntry(foodEntry.user.id, foodEntry.id, getFoodEntry()).catch(e => {
            alertService.error(e);
        });

        alertService.success("Updated", "The entry is updated");
        closeModal();
    }

    const getFoodEntry = () => {
        return {
            date: new Date(foodEntry.date),
            foodName: foodEntry.foodName,
            calorie: +foodEntry.calorie
        };
    }

    const closeModal = () => {
        setError({});
        setFoodEntry({ initial: true, user: { id: 'me' }, date: moment(new Date()).format('yyyy-MM-DD'), foodName: '', calorie: '', cheatMeal: false });
        setRequest({ loading: false });
        close();
    }

    if (!isOpen)
        return <></>;

    return (
        <div className="fixed inset-0 z-30 flex items-end bg-black bg-opacity-50 sm:items-center sm:justify-center">
            <div className="overlay close-modal"></div>
            <div className="w-full px-6 py-4 overflow-hidden bg-white rounded-t-lg dark:bg-gray-800 sm:rounded-lg sm:m-4 sm:max-w-xl">
                <div className="modal-content shadow-lg p-5">
                    <div className="border-b p-2 pb-3 pt-0 mb-4">
                        <div className="flex justify-between items-center">
                            Add Food Entry
                            <span className="close-modal cursor-pointer px-3 py-1">
                                <button onClick={closeModal}>
                                    <span className="material-icons material-icons-outlined">close</span> <span className=""></span>
                                </button>
                            </span>
                        </div>
                    </div>

                    <div className="px-4 py-3 mb-8 bg-white rounded-lg shadow-md">
                        <form onSubmit={saveFoodEntry}>
                            <label className="block text-sm">
                                <span className="text-gray-700">Date</span>
                                <input name="date" className="block w-full mt-1 text-sm focus:shadow-outline-purple form-input" type="date"
                                    value={foodEntry.date} onChange={valueChanged} />
                                {error.date && <span className="text-xs text-red-600">Please select a date.</span>}
                            </label>

                            {isAdmin && foodEntry.user && <label className="block mt-4 text-sm">
                                <span className="text-gray-700">User</span>
                                <select name="userId" className="block w-full mt-1 text-sm form-select focus:border-purple-400 focus:outline-none focus:shadow-outline-purple"
                                    value={foodEntry.user.id} onChange={valueChanged}>
                                    <option value="">Select a user</option>
                                    {users.map(u => <option value={u.id} key={u.id}>{u.name}</option>)}
                                </select>
                                {error.userId && <span className="text-xs text-red-600">Please select a user.</span>}
                            </label>}

                            <label className="block mt-4 text-sm">
                                <span className="text-gray-700">Food Name</span>
                                <input name="foodName" className="block w-full mt-1 text-sm focus:shadow-outline-purple form-input" placeholder="Enter the food name"
                                    value={foodEntry.foodName} onChange={valueChanged} />
                                {error.foodName && <span className="text-xs text-red-600">Please enter the food name.</span>}
                            </label>

                            <div className="flex mt-4 text-sm">
                                <label className="flex items-center">
                                    <input type="checkbox" name="cheatMeal" className="text-purple-600 form-checkbox focus:border-purple-400 focus:outline-none focus:shadow-outline-purple"
                                        checked={foodEntry.cheatMeal} onChange={valueChanged} />
                                    <span className="ml-2">
                                        Cheat meal?
                                    </span>
                                </label>
                            </div>

                            <label className="block mt-4 text-sm">
                                <span className="text-gray-700">Calorie</span>
                                <input name="calorie" className={"block w-full mt-1 text-sm focus:shadow-outline-purple form-input" + (foodEntry.cheatMeal ? ' opacity-50' : '')} type="number" placeholder="Enter the calorie"
                                    value={foodEntry.calorie} onChange={valueChanged} disabled={foodEntry.cheatMeal} />
                                {error.calorie && <span className="text-xs text-red-600">Please enter the calorie.</span>}
                            </label>

                            <div className="mt-6">
                                <button
                                    disabled={request.loading}
                                    type="submit"
                                    className="px-4 py-2 text-sm font-medium leading-5 text-white transition-colors duration-150 bg-purple-600 border border-transparent rounded-lg active:bg-purple-600 hover:bg-purple-700 focus:outline-none focus:shadow-outline-purple">
                                    {request.loading ? 'Saving...' : 'Save'}
                                </button>
                                {request.error && <span className="ml-6 text-red-600">{request.error}</span>}
                                {request.success && <span className="ml-6 text-green-600">Food entry is added.</span>}
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    )
}