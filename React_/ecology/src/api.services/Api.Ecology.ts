import axios from "axios";
import { IEcologyPost } from "../models/IEcologyPost";
import { IEcologyPostViewModel } from '../dtos/IEcologyPostViewModel';

async function GetEcologyPosts(): Promise<IEcologyPost[]> {
    const response = await axios.get(
        "https://localhost:7130/api/ApiEcology/GetGirls"
    );

    const ecologyDtos = response.data as IEcologyPostViewModel[];
    return ecologyDtos.map((dto) => {
        return {
            id: dto.id,
            text: dto.text,
            url: dto.imageSrc,
            likesCount: dto.likeCount,
        } as IEcologyPost;
    });
}

async function CreatePost(ecopost: IEcologyPost) {
    axios.post("https://localhost:7130/api/ApiEcology/CreateGirlForGuess", ecopost);
}

const apiEcologyChat = { GetEcologyPosts, CreatePost };

export { apiEcologyChat };
