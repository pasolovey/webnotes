echo Start building
out_dir=$1
echo OUT DIR = $out_dir
sleep 5
if [ -z "$out_dir" ]
then
    echo out_folder_isnot_defined
    echo $out_dir
    exit
fi
sh build_cake.sh -target=BuildAndPublish -out="$out_dir"
echo End building
